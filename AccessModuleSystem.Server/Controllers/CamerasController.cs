using AccessModuleSystem.Contracts.Camera;
using AccessModuleSystem.Models;
using AccessModuleSystem.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OpenCvSharp;

namespace AccessModuleSystem.Server.Controllers;

/// <summary>
/// Контроллер работы с камерами
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class CamerasController : Controller
{
  /// <summary>
  /// Контекст базы данных
  /// </summary>
  private readonly DatabaseContext _context;

  /// <summary>
  /// Конструктор класса <see cref="CamerasController"/>
  /// </summary>
  /// <param name="context"></param>
  public CamerasController(DatabaseContext context)
  {
    _context = context;
  }

  /// <summary>
  /// Получить список камер
  /// </summary>
  /// <returns></returns>
  [HttpGet]
  public async Task<ActionResult<IEnumerable<CameraReadDTO>>> GetCameras()
  {
    var cameras = await _context.Cameras
        .Select(camera => new CameraReadDTO
        {
          Id = camera.Id,
          Status = camera.Status,
          Location = camera.Location,
        })
        .ToListAsync();

    return Ok(cameras);
  }

  /// <summary>
  /// Получить камеру по идентификатору
  /// </summary>
  /// <param name="id"></param>
  /// <returns></returns>
  [HttpGet("{id}")]
  public async Task<ActionResult<CameraReadDTO>> GetCamera(Guid id)
  {
    var accessEvent = await _context.Cameras.FirstOrDefaultAsync(x => x.Id == id);

    if (accessEvent == null)
    {
      return NotFound();
    }

    return Ok(new CameraReadDTO
    {
      Id = accessEvent.Id,
      Status = accessEvent.Status,
      Location = accessEvent.Location,
    });
  }

  /// <summary>
  /// Создать камеру
  /// </summary>
  /// <param name="cameraCreateDTO"></param>
  /// <returns></returns>
  [HttpPost]
  public async Task<ActionResult<CameraReadDTO>> CreateCamera(CameraCreateDTO cameraCreateDTO)
  {
    var camera = new Camera
    {
      Id = Guid.NewGuid(),
      Location = cameraCreateDTO.Location,
      Status = cameraCreateDTO.Status,
    };

    _context.Cameras.Add(camera);
    await _context.SaveChangesAsync();

    return CreatedAtAction(nameof(GetCamera), new { id = camera.Id }, new CameraReadDTO
    {
      Id = camera.Id,
      Status = camera.Status,
      Location = camera.Location,
    });
  }

  [HttpGet("stream/{id}")]
  public async Task GetStream(Guid id)
  {
    Response.ContentType = "multipart/x-mixed-replace; boundary=--frame";
    const string boundary = "--frame";

    var videoPath = "Assets/camera_simulation.mp4";
    using var video = new VideoCapture(videoPath);
    using var httpClient = new HttpClient();
    var frame = new Mat();
    var cancellationToken = HttpContext.RequestAborted;

    YoloDetectionResult? lastYoloDetections = null;
    var yoloLock = new object();
    var inferenceTask = Task.CompletedTask;

    try
    {
      while (!cancellationToken.IsCancellationRequested && video.IsOpened())
      {
        video.Read(frame);
        if (frame.Empty()) break;

        var currentFrame = frame.Clone();

        if (inferenceTask.IsCompleted)
        {
          inferenceTask = Task.Run(async () =>
          {
            var detections = await RunYoloInferenceAsync(currentFrame, httpClient);
            lock (yoloLock)
            {
              lastYoloDetections = detections;
            }
            if (detections != null)
            {
              await TrySaveAccessEventAsync(id, currentFrame, detections);
            }
          }, cancellationToken);
        }

        lock (yoloLock)
        {
          if (lastYoloDetections != null)
          {
            DrawDetections(frame, lastYoloDetections);
          }
        }

        Cv2.ImEncode(".jpg", frame, out var imageBytes);
        await WriteMjpegFrame(Response, boundary, imageBytes);

        await Task.Delay(1000 / 60, cancellationToken);
      }
    }
    catch (TaskCanceledException)
    {
      Console.WriteLine($"Стрим камеры {id} остановлен клиентом.");
    }
    catch (Exception ex)
    {
      Console.WriteLine($"Ошибка в MJPEG потоке камеры {id}: {ex.Message}");
    }
  }

  private async Task TrySaveAccessEventAsync(Guid cameraId, Mat frame, YoloDetectionResult detections)
  {
    var now = DateTime.UtcNow;
    var tenMinutesAgo = now.AddMinutes(-10);

    var classificationNames = detections.Detections
        .Where(d => d.ClassId is not null && !string.IsNullOrEmpty(d.ClassName))
        .Select(d => d.ClassName!.ToLowerInvariant())
        .ToList();

    var plateDetection = detections.Detections
        .FirstOrDefault(d => !string.IsNullOrEmpty(d.RecognizedText));

    var plate = plateDetection?.RecognizedText?.Trim();
    bool plateIsUnknown = string.IsNullOrEmpty(plate) || plate == "Не удалось распознать";

    var existingEvent = await _context.AccessEvents
        .Where(e => e.Vehicle.LicensePlate == plate && e.Timestamp >= tenMinutesAgo)
        .FirstOrDefaultAsync();

    if (existingEvent != null)
      return;

    var eventType = AccessEventType.Detection;
    if (classificationNames.Contains("ambulance") || classificationNames.Contains("ambulance_sign"))
      eventType = AccessEventType.Classification;

    var status = AccessStatus.Granted;
    if (eventType == AccessEventType.Detection && classificationNames.All(n => n == "none"))
    {
      if (plateIsUnknown || !await _context.Vehicles.AnyAsync(v => v.LicensePlate == plate))
      {
        status = AccessStatus.Denied;
      }
    }

    Cv2.ImEncode(".jpg", frame, out var imageBytes);

    var vehicle = await _context.Vehicles.FirstOrDefaultAsync(v => v.LicensePlate == plate);
    if (vehicle == null)
    {
      vehicle = new Vehicle
      {
        Id = Guid.NewGuid(),
        LicensePlate = plate ?? "UNKNOWN",
        OwnerName = "Неизвестно",
        Status = PermissionStatus.Active,
        CreatedAt = now
      };
      _context.Vehicles.Add(vehicle);
    }

    var accessEvent = new AccessEvent
    {
      Id = Guid.NewGuid(),
      Timestamp = now,
      AccessType = AccessType.Entry,
      Status = status,
      Vehicle = vehicle,
      CameraId = cameraId,
      EventType = eventType,
      VehicleId = vehicle.Id,
      Screenshot = imageBytes.ToArray()
    };

    _context.AccessEvents.Add(accessEvent);
    await _context.SaveChangesAsync();
  }

  private async Task<YoloDetectionResult?> RunYoloInferenceAsync(Mat frame, HttpClient httpClient)
  {
    var tempPath = Path.GetTempFileName() + ".jpg";
    Cv2.ImWrite(tempPath, frame);

    try
    {
      using var imageContent = new StreamContent(System.IO.File.OpenRead(tempPath));
      imageContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/jpeg");
      using var form = new MultipartFormDataContent();
      form.Add(imageContent, "file", "frame.jpg");

      var classificationTask = httpClient.PostAsync("http://localhost:8000/classification", form);
      var detectionTask = httpClient.PostAsync("http://localhost:8000/detect", form);

      await Task.WhenAll(classificationTask, detectionTask);

      var result = new YoloDetectionResult();

      if (classificationTask.Result.IsSuccessStatusCode)
      {
        var classResult = await classificationTask.Result.Content.ReadFromJsonAsync<YoloDetectionResult>();
        if (classResult != null)
          result.Detections.AddRange(classResult.Detections);
      }

      if (detectionTask.Result.IsSuccessStatusCode)
      {
        var detectResult = await detectionTask.Result.Content.ReadFromJsonAsync<YoloDetectionResult>();
        if (detectResult != null)
          result.Detections.AddRange(detectResult.Detections);
      }

      return result;
    }
    catch (Exception ex)
    {
      Console.WriteLine("Ошибка YOLO: " + ex.Message);
    }
    finally
    {
      System.IO.File.Delete(tempPath);
    }

    return null;
  }

  private void DrawDetections(Mat frame, YoloDetectionResult detections)
  {
    foreach (var det in detections.Detections)
    {
      var p1 = new Point(det.Bbox[0], det.Bbox[1]);
      var p2 = new Point(det.Bbox[2], det.Bbox[3]);

      var color = det.RecognizedText != null
          ? new Scalar(0, 0, 255)
          : new Scalar(0, 255, 0);

      frame.Rectangle(p1, p2, color, 2);

      var label = det.RecognizedText != null
          ? $"plate: {det.RecognizedText}"
          : $"{det.ClassName} {det.Confidence * 100:F0}%";

      var textPos = new Point(p1.X, Math.Max(15, p1.Y - 10));
      frame.PutText(label, textPos, HersheyFonts.HersheySimplex, 0.6, new Scalar(255, 255, 255), 1);
    }
  }

  private async Task WriteMjpegFrame(HttpResponse response, string boundary, byte[] imageData)
  {
    await response.WriteAsync(boundary + "\r\n");
    await response.WriteAsync("Content-Type: image/jpeg\r\n");
    await response.WriteAsync($"Content-Length: {imageData.Length}\r\n\r\n");
    await response.Body.WriteAsync(imageData, 0, imageData.Length);
    await response.WriteAsync("\r\n");
    await response.Body.FlushAsync();
  }

  private class YoloDetectionResult
  {
    public List<Detection> Detections { get; set; } = new();
  }

  private class Detection
  {
    public int? ClassId { get; set; }
    public string ClassName { get; set; } = "";
    public float Confidence { get; set; }
    public List<int> Bbox { get; set; } = new();
    public string? RecognizedText { get; set; }
  }
}
