using AccessModuleSystem.Client.Models.Enums;

namespace AccessModuleSystem.Client.Models;

/// <summary>
/// Транспортные средства, зарегистрированные в системе
/// </summary>
public class Vehicles
{
  /// <summary>
  /// Уникальный идентификатор
  /// </summary>
  public Guid Id { get; set; }

  /// <summary>
  /// Номерной знак автомобиля
  /// </summary>
  public string LicensePlate { get; set; } = string.Empty;

  /// <summary>
  /// Имя владельца
  /// </summary>
  public string OwnerName { get; set; } = string.Empty;

  /// <summary>
  /// Статус разрешения
  /// </summary>
  public PermissionStatus Status { get; set; }

  /// <summary>
  /// Дата добавления
  /// </summary>
  public DateTime CreatedAt { get; set; }
}
