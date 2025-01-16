using System.ComponentModel.DataAnnotations;

namespace AccessModuleSystem.Client.Models.Enums;

/// <summary>
/// Статус состояния камеры
/// </summary>
public enum CameraStatus
{
  /// <summary>
  /// Активный
  /// </summary>
  [Display(Name = "Активный")]
  Active = 0,

  /// <summary>
  /// Неактивный
  /// </summary>
  [Display(Name = "Неактивный")]
  Inactive = 1,

  /// <summary>
  /// Обслуживание
  /// </summary>
  [Display(Name = "Обслуживание")]
  Maintenance = 2,

  /// <summary>
  /// Ошибка
  /// </summary>
  [Display(Name = "Ошибка")]
  Error = 3
}
