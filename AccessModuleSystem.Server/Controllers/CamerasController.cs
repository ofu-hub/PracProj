using AccessModuleSystem.Contracts.AccessEvent;
using AccessModuleSystem.Contracts.Camera;
using AccessModuleSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

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

    using var video = new OpenCvSharp.VideoCapture(videoPath);
    using var httpClient = new HttpClient();
    var frame = new OpenCvSharp.Mat();
    var cancellationToken = HttpContext.RequestAborted;

    YoloDetectionResult? lastYoloDetections = null;
    var yoloLock = new object();

    // Запускаем фоновую задачу для YOLO
    var inferenceTask = Task.CompletedTask;

    try
    {
      while (!cancellationToken.IsCancellationRequested && video.IsOpened())
      {
        video.Read(frame);
        if (frame.Empty()) break;

        var currentFrame = frame.Clone();

        // YOLO — асинхронно
        if (inferenceTask.IsCompleted)
        {
          inferenceTask = Task.Run(async () =>
          {
            var detections = await RunYoloInferenceAsync(currentFrame, httpClient);
            lock (yoloLock)
            {
              lastYoloDetections = detections;
            }
          }, cancellationToken);
        }

        // Применяем последнюю разметку
        lock (yoloLock)
        {
          if (lastYoloDetections != null)
          {
            DrawDetections(frame, lastYoloDetections);
          }
        }

        // Кодируем и отправляем кадр
        OpenCvSharp.Cv2.ImEncode(".jpg", frame, out var imageBytes);
        await WriteMjpegFrame(Response, boundary, imageBytes);

        await Task.Delay(1000 / 60, cancellationToken); // ~60 FPS
      }
    }
    catch (TaskCanceledException)
    {
      Console.WriteLine($"Стрим камеры {id} остановлен клиентом (отключился).");
    }
    catch (Exception ex)
    {
      Console.WriteLine($"Ошибка в MJPEG потоке камеры {id}: {ex.Message}");
    }
    finally
    {
      Console.WriteLine($"Стрим камеры {id} завершён.");
    }
  }


  private async Task<YoloDetectionResult?> RunYoloInferenceAsync(OpenCvSharp.Mat frame, HttpClient httpClient)
  {
    var tempPath = Path.GetTempFileName() + ".jpg";
    OpenCvSharp.Cv2.ImWrite(tempPath, frame);

    try
    {
      using var imageContent = new StreamContent(System.IO.File.OpenRead(tempPath));
      imageContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/jpeg");

      using var form = new MultipartFormDataContent();
      form.Add(imageContent, "file", "frame.jpg");

      // Запросы параллельно
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
        var detectResponse = await detectionTask.Result.Content.ReadFromJsonAsync<YoloDetectionResult>();
        if (detectResponse != null)
          result.Detections.AddRange(detectResponse.Detections);
      }

      return result;
    }
    catch (Exception ex)
    {
      Console.WriteLine("Ошибка запроса к YOLO API: " + ex.Message);
    }
    finally
    {
      System.IO.File.Delete(tempPath);
    }

    return null;
  }


  private void DrawDetections(OpenCvSharp.Mat frame, YoloDetectionResult detections)
  {
    var selected_detections = detections.Detections
        .Where(d => d.ClassId == 0 || !string.IsNullOrEmpty(d.RecognizedText))
        .ToList();

    foreach (var det in detections.Detections)
    {
      var p1 = new OpenCvSharp.Point(det.Bbox[0], det.Bbox[1]);
      var p2 = new OpenCvSharp.Point(det.Bbox[2], det.Bbox[3]);

      var color = det.RecognizedText != null
          ? new OpenCvSharp.Scalar(0, 0, 255)   // красный для номеров
          : new OpenCvSharp.Scalar(0, 255, 0);  // зелёный для машин

      frame.Rectangle(p1, p2, color, 2);

      var label = det.RecognizedText != null
          ? $"plate: {det.RecognizedText}"
          : $"{det.ClassName} {det.Confidence * 100:F0}%";

      var textPos = new OpenCvSharp.Point(p1.X, Math.Max(15, p1.Y - 10));

      frame.PutText(label,
          textPos,
          OpenCvSharp.HersheyFonts.HersheySimplex,
          0.6,
          new OpenCvSharp.Scalar(255, 255, 255), 1);
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
}

public class YoloDetectionResult
{
  public List<Detection> Detections { get; set; } = new();
}

public class Detection
{
  public int? ClassId { get; set; }
  public string ClassName { get; set; } = "";
  public float Confidence { get; set; }
  public List<int> Bbox { get; set; } = new();
  public string? RecognizedText { get; set; }
}
