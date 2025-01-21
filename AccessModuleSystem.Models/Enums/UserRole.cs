using System.ComponentModel.DataAnnotations;

namespace AccessModuleSystem.Models.Enums;

/// <summary>
/// Роли пользователей
/// </summary>
public enum UserRole
{
  /// <summary>
  /// Администратор
  /// </summary>
  [Display(Name = "Администратор")]
  Admin = 0,

  /// <summary>
  /// Пользователь
  /// </summary>
  [Display(Name = "Пользователь")]
  User = 1,

  /// <summary>
  /// Оператор
  /// </summary>
  [Display(Name = "Оператор")]
  Operator = 2,

  /// <summary>
  /// Инженер
  /// </summary>
  [Display(Name = "Инженер")]
  Engineer = 3
}
