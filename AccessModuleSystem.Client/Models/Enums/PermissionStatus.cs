using System.ComponentModel.DataAnnotations;

namespace AccessModuleSystem.Client.Models.Enums;

/// <summary>
/// Статус разрешения
/// </summary>
public enum PermissionStatus
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
  Inactive = 1
}
