using AccessModuleSystem.Contracts.Vehicle;
using AccessModuleSystem.Contracts.User;
using AccessModuleSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Data;
using System.Xml.Linq;

namespace AccessModuleSystem.Server.Controllers;

/// <summary>
/// Контроллер работы с транспортными средствами
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class VehiclesController : ControllerBase
{
  /// <summary>
  /// Контекст базы данных
  /// </summary>
  private readonly DatabaseContext _context;

  /// <summary>
  /// Конструктор класса <see cref="VehiclesController"/>
  /// </summary>
  /// <param name="context"></param>
  public VehiclesController(DatabaseContext context)
  {
    _context = context;
  }

  /// <summary>
  /// Получить список транспортных средств
  /// </summary>
  /// <returns></returns>
  [HttpGet]
  public async Task<ActionResult<IEnumerable<VehicleReadDTO>>> GetVehicles()
  {
    var vehicles = await _context.Vehicles
        .Select(vehicle => new VehicleReadDTO
        {
          Id = vehicle.Id,
          LicensePlate = vehicle.LicensePlate,
          OwnerName = vehicle.OwnerName,
          Status = vehicle.Status,
          CreatedAt = vehicle.CreatedAt,
          DeactivationAt = vehicle.DeactivationAt
        })
        .ToListAsync();

    return Ok(vehicles);
  }

  /// <summary>
  /// Получить транспортное средство по идентификатору
  /// </summary>
  /// <param name="id"></param>
  /// <returns></returns>
  [HttpGet("{id}")]
  public async Task<ActionResult<VehicleDetailDTO>> GetVehicle(Guid id)
  {
    var vehicle = await _context.Vehicles.Include(x => x.User).FirstOrDefaultAsync(x => x.Id == id);

    if (vehicle == null)
    {
      return NotFound();
    }

    var userRead = new UserReadDTO();
    if (vehicle.User is not null)
    {
      userRead.Id = vehicle.User.Id;
      userRead.Username = vehicle.User.Username;
      userRead.Name = vehicle.User.Name;
      userRead.Surname = vehicle.User.Surname;
      userRead.Patronymic = vehicle.User.Patronymic;
      userRead.Email = vehicle.User.Email;
      userRead.Role = vehicle.User.Role;
      userRead.CreatedAt = vehicle.User.CreatedAt;
      userRead.IsBlocked = vehicle.User.IsBlocked;
    }
    else
    {
      userRead = null;
    }

    return Ok(new VehicleDetailDTO
    {
      Id = vehicle.Id,
      LicensePlate = vehicle.LicensePlate,
      OwnerName = vehicle.OwnerName,
      Status = vehicle.Status,
      CreatedAt = vehicle.CreatedAt,
      DeactivationAt = vehicle.DeactivationAt,
      User = userRead
    });
  }

  /// <summary>
  /// Создать транспортное средство
  /// </summary>
  /// <param name="vehicleCreateDTO"></param>
  /// <returns></returns>
  [HttpPost]
  public async Task<ActionResult<VehicleReadDTO>> CreateVehicle(VehicleCreateDTO vehicleCreateDTO)
  {
    var vehicle = new Vehicle
    {
      Id = Guid.NewGuid(),
      LicensePlate = vehicleCreateDTO.LicensePlate,
      OwnerName = vehicleCreateDTO.OwnerName,
      Status = vehicleCreateDTO.Status,
      CreatedAt = DateTime.UtcNow,
      DeactivationAt = vehicleCreateDTO.DeactivationAt
    };

    _context.Vehicles.Add(vehicle);
    await _context.SaveChangesAsync();

    return CreatedAtAction(nameof(GetVehicle), new { id = vehicle.Id }, new VehicleReadDTO
    {
      Id = vehicle.Id,
      LicensePlate = vehicle.LicensePlate,
      OwnerName = vehicle.OwnerName,
      Status = vehicle.Status,
      CreatedAt = vehicle.CreatedAt,
      DeactivationAt = vehicle.DeactivationAt
    });
  }

  /// <summary>
  /// Обновить транспортное средство
  /// </summary>
  /// <param name="id"></param>
  /// <param name="vehicleUpdateDTO"></param>
  /// <returns></returns>
  [HttpPut("{id}")]
  public async Task<IActionResult> UpdateVehicle(Guid id, VehicleUpdateDTO vehicleUpdateDTO)
  {
    var vehicle = await _context.Vehicles.FindAsync(id);

    if (vehicle == null)
    {
      return NotFound();
    }

    vehicle.LicensePlate = vehicleUpdateDTO.LicensePlate;
    vehicle.OwnerName = vehicleUpdateDTO.OwnerName;
    vehicle.Status = vehicleUpdateDTO.Status;
    vehicle.DeactivationAt = vehicleUpdateDTO.DeactivationAt;

    _context.Entry(vehicle).State = EntityState.Modified;
    await _context.SaveChangesAsync();

    return NoContent();
  }

  /// <summary>
  /// Удалить транспортное средство
  /// </summary>
  /// <param name="id"></param>
  /// <returns></returns>
  [HttpDelete("{id}")]
  public async Task<IActionResult> DeleteVehicle(Guid id)
  {
    var vehicle = await _context.Vehicles.FindAsync(id);

    if (vehicle == null)
    {
      return NotFound();
    }

    _context.Vehicles.Remove(vehicle);
    await _context.SaveChangesAsync();

    return NoContent();
  }
}
