using AccessModuleSystem.Models.Enums;

namespace AccessModuleSystem.Contracts.Camera;

/// <summary>
/// DTO для создания новой камеры
/// </summary>
public class CameraCreateDTO
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