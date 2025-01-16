using System.ComponentModel.DataAnnotations;

namespace PracProj.Models.Enums;

/// <summary>
/// Статус доступа
/// </summary>
public enum AccessStatus
{
  /// <summary>
  /// Предоставленный
  /// </summary>
  [Display(Name = "Предоставленный")] 
  Granted = 0,

  /// <summary>
  /// Отклонен
  /// </summary>
  [Display(Name = "Отклонен")] 
  Denied = 1
}
