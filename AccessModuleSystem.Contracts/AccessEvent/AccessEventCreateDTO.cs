﻿using AccessModuleSystem.Models.Enums;

namespace AccessModuleSystem.Contracts.AccessEvent;

/// <summary>
/// DTO для создания нового события доступа
/// </summary>
public class AccessEventCreateDTO
{
  /// <summary>
  /// Идентификатор транспортного средства
  /// </summary>
  public Guid VehicleId { get; set; }

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
  /// Идентификатор камеры, зафиксировавшей событие
  /// </summary>
  public Guid CameraId { get; set; }
}