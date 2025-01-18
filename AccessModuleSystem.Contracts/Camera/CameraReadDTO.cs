using AccessModuleSystem.Models.Enums;

namespace AccessModuleSystem.Contracts.Camera;

/// <summary>
/// DTO для чтения информации о камерах
/// </summary>
public class CameraReadDTO
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