using AccessModuleSystem.Models.Enums;

namespace AccessModuleSystem.Contracts.AccessEvent;

/// <summary>
/// DTO для чтения информации о событиях доступа
/// </summary>
public class AccessEventReadDTO
{
  /// <summary>
  /// Уникальный идентификатор события
  /// </summary>
  public Guid Id { get; set; }

  /// <summary>
  /// Номерной знак автомобиля
  /// </summary>
  public string LicensePlate { get; set; } = string.Empty;

  /// <summary>
  /// Идентификатор транспортного средства
  /// </summary>
  public Guid? VehicleId { get; set; }

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

  /// <summary>
  /// Тип события (детекция/классификация)
  /// </summary>
  public AccessEventType EventType { get; set; }
}