using System.ComponentModel.DataAnnotations;

namespace PracProj.Models.Enums;

/// <summary>
/// Тип события
/// </summary>
public enum AccessType
{
  /// <summary>
  /// Въезд
  /// </summary>
  [Display(Name = "Въезд")]
  Entry = 0,

  /// <summary>
  /// Выезд
  /// </summary>
  [Display(Name = "Выезд")]
  Exit = 1
}
