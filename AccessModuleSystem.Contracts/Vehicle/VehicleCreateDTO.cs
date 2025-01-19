using AccessModuleSystem.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace AccessModuleSystem.Contracts.Vehicle;

/// <summary>
/// DTO для создания нового транспортного средства
/// </summary>
public class VehicleCreateDTO
{
  /// <summary>
  /// Номерной знак автомобиля
  /// </summary>
  [Required(ErrorMessage = "Номерной знак обязателен.")]
  [StringLength(14, MinimumLength = 6, ErrorMessage = "Номерной знак должен быть от 6 до 14 символов.")]
  public string LicensePlate { get; set; } = string.Empty;

  /// <summary>
  /// Имя владельца
  /// </summary>
  [Required(ErrorMessage = "Имя владельца обязательно.")]
  [StringLength(50, MinimumLength = 2, ErrorMessage = "Имя владельца должно быть от 2 до 50 символов.")]
  public string OwnerName { get; set; } = string.Empty;

  /// <summary>
  /// Статус разрешения
  /// </summary>
  public PermissionStatus Status { get; set; } = PermissionStatus.Active;

  /// <summary>
  /// Дата деактивации
  /// </summary>
  public DateTime? DeactivationAt { get; set; }
}
