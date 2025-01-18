using AccessModuleSystem.Contracts.Camera;
using AccessModuleSystem.Contracts.Vehicle;
using AccessModuleSystem.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccessModuleSystem.Contracts.AccessEvent;

/// <summary>
/// DTO для просмотра события въезда или выезда транспорта
/// </summary>
public class AccessEventDetailDTO
{
  /// <summary>
  /// Уникальный идентификатор события
  /// </summary>
  public Guid Id { get; set; }

  /// <summary>
  /// Время события
  /// </summary>
  public DateTime Timestamp { get; set; }

  /// <summary>
  /// Тип события (въезд/выезд)
  /// </summary>
  public AccessType AccessType { get; set; }

  /// <summary>
  /// Статус доступа
  /// </summary>
  public AccessStatus Status { get; set; }

  /// <summary>
  /// Информация о транспортном средстве
  /// </summary>
  public VehicleReadDTO Vehicle { get; set; } = null!;

  /// <summary>
  /// Информация о камере
  /// </summary>
  public CameraReadDTO Camera { get; set; } = null!;
}