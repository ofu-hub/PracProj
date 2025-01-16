using System.ComponentModel.DataAnnotations;

namespace AccessModuleSystem.Client.Models.Enums;

public enum UserRole
{
  [Display(Name = "Администратор")]
  Admin,

  [Display(Name = "Пользователь")]
  User,

  [Display(Name = "Оператор")]
  Operator,

  [Display(Name = "Инженер")]
  Engineer
}
