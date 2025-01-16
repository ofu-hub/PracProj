using PracProj.Models.Enums;

namespace PracProj.Models.Events;

/// <summary>
/// Модель представления въезда или выезда с территории
/// </summary>
public class ViewAccessEvents
{
  /// <summary>
  /// Уникальный идентификатор
  /// </summary>
  public Guid Id { get; set; }

  /// <summary>
  /// Идентификатор авто
  /// </summary>
  public Guid VehicleId { get; set; }

  /// <summary>
  /// Номерной знак автомобиля
  /// </summary>
  public string LicensePlate { get; set; } = string.Empty;

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
}
