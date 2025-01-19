using AccessModuleSystem.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace AccessModuleSystem.Contracts.Camera;

/// <summary>
/// DTO для создания новой камеры
/// </summary>
public class CameraCreateDTO
{
  /// <summary>
  /// Местоположение камеры
  /// </summary>
  [Required(ErrorMessage = "Местоположение камеры обязательно.")]
  [StringLength(100, MinimumLength = 2, ErrorMessage = "Местоположение камеры должно быть от 5 до 100 символов.")]
  public string Location { get; set; } = string.Empty;

  /// <summary>
  /// Состояние камеры
  /// </summary>
  public CameraStatus Status { get; set; }
}