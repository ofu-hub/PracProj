using AccessModuleSystem.Models.Enums;

namespace AccessModuleSystem.Contracts.Vehicle;

/// <summary>
/// DTO для чтения информации о транспортных средствах
/// </summary>
public class VehicleReadDTO
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