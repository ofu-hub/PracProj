using AccessModuleSystem.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccessModuleSystem.Contracts.User;

/// <summary>
/// DTO для создания нового пользователя
/// </summary>
public class UserCreateDTO
{
  /// <summary>
  /// Имя пользователя
  /// </summary>
  public string Username { get; set; } = string.Empty;

  /// <summary>
  /// Имя
  /// </summary>
  public string Name { get; set; } = string.Empty;

  /// <summary>
  /// Фамилия
  /// </summary>
  public string Surname { get; set; } = string.Empty;

  /// <summary>
  /// Отчество
  /// </summary>
  public string? Patronymic { get; set; }

  /// <summary>
  /// Почтовый электронный адрес
  /// </summary>
  public string Email { get; set; } = string.Empty;

  /// <summary>
  /// Пароль
  /// </summary>
  public string Password { get; set; } = string.Empty;

  /// <summary>
  /// Роль пользователя
  /// </summary>
  public UserRole Role { get; set; }
}