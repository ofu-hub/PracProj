using AccessModuleSystem.Contracts.User;
using AccessModuleSystem.Models.Enums;

namespace AccessModuleSystem.Contracts.Vehicle;

/// <summary>
/// DTO для детальной информации о транспортных средствах
/// </summary>
public class VehicleDetailDTO
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
  /// Пользователь
  /// </summary>
  public UserReadDTO? User { get; set; }
}
