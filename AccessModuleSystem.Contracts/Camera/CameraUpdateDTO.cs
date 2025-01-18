using AccessModuleSystem.Models.Enums;

namespace AccessModuleSystem.Contracts.Camera;

/// <summary>
/// DTO для обновления информации о камерах
/// </summary>
public class CameraUpdateDTO
{
  /// <summary>
  /// Местоположение камеры
  /// </summary>
  public string Location { get; set; } = string.Empty;

  /// <summary>
  /// Состояние камеры
  /// </summary>
  public CameraStatus Status { get; set; }
}