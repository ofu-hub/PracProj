using AccessModuleSystem.Models.Enums;

namespace AccessModuleSystem.Models;

/// <summary>
/// Транспортные средства, зарегистрированные в системе
/// </summary>
public class Vehicle
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

  /// <summary>
  /// Дата деактивации/удаления
  /// </summary>
  public DateTime? DeactivationAt { get; set; }

  /// <summary>
  /// Идентификатор пользователя
  /// </summary>
  public Guid? UserId { get; set; }

  /// <summary>
  /// Пользователь
  /// </summary>
  public virtual User? User { get; set; }
}
