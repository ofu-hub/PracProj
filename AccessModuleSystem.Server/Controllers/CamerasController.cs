using AccessModuleSystem.Contracts.AccessEvent;
using AccessModuleSystem.Contracts.Camera;
using AccessModuleSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
  public async Task<ActionResult<AccessEventReadDTO>> CreateAccessEvent(CameraCreateDTO cameraCreateDTO)
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
}
