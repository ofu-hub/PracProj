using AccessModuleSystem.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace AccessModuleSystem.Contracts.User;

/// <summary>
/// DTO для создания нового пользователя
/// </summary>
public class UserCreateDTO
{
  /// <summary>
  /// Имя пользователя
  /// </summary>
  [Required(ErrorMessage = "Имя пользователя обязательно.")]
  [StringLength(50, MinimumLength = 3, ErrorMessage = "Имя пользователя должно быть от 3 до 50 символов.")]
  public string Username { get; set; } = string.Empty;

  /// <summary>
  /// Имя
  /// </summary>
  [Required(ErrorMessage = "Имя обязательно.")]
  public string Name { get; set; } = string.Empty;

  /// <summary>
  /// Фамилия
  /// </summary>
  [Required(ErrorMessage = "Фамилия обязательна.")]
  public string Surname { get; set; } = string.Empty;

  /// <summary>
  /// Отчество
  /// </summary>
  public string? Patronymic { get; set; }

  /// <summary>
  /// Почтовый электронный адрес
  /// </summary>
  [Required(ErrorMessage = "Электронная почта обязательна.")]
  [EmailAddress(ErrorMessage = "Введите корректный адрес электронной почты.")]
  public string Email { get; set; } = string.Empty;

  /// <summary>
  /// Пароль
  /// </summary>
  [Required(ErrorMessage = "Пароль обязателен.")]
  [StringLength(100, MinimumLength = 6, ErrorMessage = "Пароль должен быть не менее 6 символов.")]
  public string Password { get; set; } = string.Empty;

  /// <summary>
  /// Роль пользователя
  /// </summary>
  [Required(ErrorMessage = "Роль обязательна.")]
  public UserRole Role { get; set; }
}