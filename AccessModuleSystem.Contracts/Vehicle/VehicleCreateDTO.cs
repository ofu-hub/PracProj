using AccessModuleSystem.Models.Enums;

namespace AccessModuleSystem.Contracts.Vehicle;

/// <summary>
/// DTO для создания нового транспортного средства
/// </summary>
public class VehicleCreateDTO
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