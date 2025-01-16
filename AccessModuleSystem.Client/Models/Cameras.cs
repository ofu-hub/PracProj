using AccessModuleSystem.Client.Models.Enums;

namespace AccessModuleSystem.Client.Models;

/// <summary>
/// Камеры на территории жилого комплекса
/// </summary>
public class Cameras
{
  /// <summary>
  /// Уникальный идентификатор
  /// </summary>
  public Guid Id { get; set; }

  /// <summary>
  /// Местоположение камеры
  /// </summary>
  public string Location { get; set; } = string.Empty;

  /// <summary>
  /// Состояние камеры
  /// </summary>
  public CameraStatus Status { get; set; }
}
