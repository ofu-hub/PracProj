using System.ComponentModel.DataAnnotations;

namespace PracProj.Models.Enums;

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
