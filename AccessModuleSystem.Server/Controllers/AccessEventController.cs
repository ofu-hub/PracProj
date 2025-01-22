using AccessModuleSystem.Contracts.AccessEvent;
using AccessModuleSystem.Contracts.Camera;
using AccessModuleSystem.Contracts.Vehicle;
using AccessModuleSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AccessModuleSystem.Server.Controllers;

/// <summary>
/// Контроллер работы с событями
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class AccessEventController : Controller
{
  /// <summary>
  /// Контекст базы данных
  /// </summary>
  private readonly DatabaseContext _context;

  /// <summary>
  /// Конструктор класса <see cref="AccessEventController"/>
  /// </summary>
  /// <param name="context"></param>
  public AccessEventController(DatabaseContext context)
  {
    _context = context;
  }

  /// <summary>
  /// Получить список событий
  /// </summary>
  /// <returns></returns>
  [HttpGet]
  public async Task<ActionResult<IEnumerable<AccessEventReadDTO>>> GetAccessEvents()
  {
    var accessEvents = await _context.AccessEvents
      .Include(e => e.Vehicle)
      .Include(x => x.Camera)
      .Select(accessEvent => new AccessEventReadDTO
      {
        Id = accessEvent.Id,
        AccessType = accessEvent.AccessType,
        CameraId = accessEvent.CameraId,
        LicensePlate = accessEvent.Vehicle.LicensePlate,
        Status = accessEvent.Status,
        Timestamp = accessEvent.Timestamp,
        VehicleId = accessEvent.VehicleId
      })
      .ToListAsync();

    return Ok(accessEvents);
  }

  /// <summary>
  /// Получить событие по идентификатору
  /// </summary>
  /// <param name="id"></param>
  /// <returns></returns>
  [HttpGet("{id:Guid}")]
  public async Task<ActionResult<AccessEventDetailDTO>> GetAccessEvent(Guid id)
  {
    var accessEvent = await _context.AccessEvents
      .Include(x => x.Vehicle)
      .Include(x => x.Camera)
      .FirstOrDefaultAsync(x => x.Id == id);


    if (accessEvent == null)
    {
      return NotFound();
    }

    return Ok(new AccessEventDetailDTO
    {
      Id = accessEvent.Id,
      AccessType = accessEvent.AccessType,
      Status = accessEvent.Status,
      Timestamp = accessEvent.Timestamp,
      Camera = new CameraReadDTO()
      {
        Id = accessEvent.Camera.Id,
        Location = accessEvent.Camera.Location,
        Status = accessEvent.Camera.Status
      },
      Vehicle = new VehicleReadDTO()
      {
        Id = accessEvent.Vehicle.Id,
        Status = accessEvent.Vehicle.Status,
        CreatedAt = accessEvent.Vehicle.CreatedAt,
        DeactivationAt = accessEvent.Vehicle.DeactivationAt,
        LicensePlate = accessEvent.Vehicle.LicensePlate,
        OwnerName = accessEvent.Vehicle.OwnerName
      }
    });
  }

  /// <summary>
  /// Создать событие
  /// </summary>
  /// <param name="accessEventCreateDTO"></param>
  /// <returns></returns>
  [HttpPost]
  public async Task<ActionResult<AccessEventReadDTO>> CreateAccessEvent(AccessEventCreateDTO accessEventCreateDTO)
  {
    var camera = await _context.Cameras.FirstOrDefaultAsync(x => x.Id == accessEventCreateDTO.CameraId);

    if (camera == null)
      return BadRequest("There is no such camera in the system.");

    var vehicle = await _context.Vehicles.FirstOrDefaultAsync(x => x.Id == accessEventCreateDTO.VehicleId);

    if (vehicle == null)
      return BadRequest("There is no such vehicle in the system.");

    var accessEvent = new AccessEvent
    {
      Id = Guid.NewGuid(),
      AccessType = accessEventCreateDTO.AccessType,
      Status = accessEventCreateDTO.Status,
      Timestamp = DateTime.UtcNow,
      CameraId = accessEventCreateDTO.CameraId,
      Camera = camera,
      VehicleId = accessEventCreateDTO.VehicleId,
      Vehicle = vehicle
    };

    _context.AccessEvents.Add(accessEvent);
    await _context.SaveChangesAsync();

    return CreatedAtAction(nameof(GetAccessEvent), new { id = accessEvent.Id }, new AccessEventReadDTO
    {
      Id = accessEvent.Id,
      AccessType = accessEvent.AccessType,
      CameraId = accessEvent.CameraId,
      LicensePlate = accessEvent.Vehicle.LicensePlate,
      Status = accessEvent.Status,
      Timestamp = accessEvent.Timestamp,
      VehicleId = accessEvent.VehicleId
    });
  }
}
