using AccessModuleSystem.Models.Enums;

namespace AccessModuleSystem.Models;

/// <summary>
/// События доступа въезда или выезда с территории
/// </summary>
public class AccessEvent
{
  /// <summary>
  /// Уникальный идентификатор
  /// </summary>
  public Guid Id { get; set; }

  /// <summary>
  /// Идентификатор транспортного средства
  /// </summary>
  public Guid? VehicleId { get; set; }

  /// <summary>
  /// Транспортное средство
  /// </summary>
  public virtual Vehicle? Vehicle { get; set; }

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
  /// Камера фиксирующая
  /// </summary>
  public Guid CameraId { get; set; }

  /// <summary>
  /// Камера
  /// </summary>
  public virtual Camera Camera { get; set; } = null!;

  /// <summary>
  /// Тип события (детекция/классификация)
  /// </summary>
  public AccessEventType EventType { get; set; }

  /// <summary>
  /// Скриншот полученный с камеры
  /// </summary>
  public byte[]? Screenshot { get; set; }
}
