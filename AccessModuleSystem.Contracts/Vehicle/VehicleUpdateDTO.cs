using AccessModuleSystem.Models.Enums;

namespace AccessModuleSystem.Contracts.Vehicle;

/// <summary>
/// DTO для обновления информации о транспортных средствах
/// </summary>
public class VehicleUpdateDTO
{
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
}