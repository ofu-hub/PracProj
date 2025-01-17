using System.ComponentModel.DataAnnotations;

namespace AccessModuleSystem.Contracts.Auth;

public class LoginDto
{
  [Required(ErrorMessage = "Введите логин!")]
  public string? Username { get; set; }

  [Required(ErrorMessage = "Введите пароль!")]
  [DataType(DataType.Password)]
  public string? Password { get; set; }
}
