using System.ComponentModel.DataAnnotations;

namespace AccessModuleSystem.Models.Enums;

/// <summary>
/// Определяет тип события
/// </summary>
public enum AccessEventType
{
  /// <summary>
  /// Детекция номерного знака
  /// </summary>
  [Display(Name = "Детекция")]
  Detection = 0,

  /// <summary>
  /// Классификация автомобиля
  /// </summary>
  [Display(Name = "Классификация")]
  Classification = 1
}
