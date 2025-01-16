using PracProj.Models.Enums;

namespace PracProj.Models;

/// <summary>
/// Пользователи системы
/// </summary>
public class Users
{
  /// <summary>
  /// Уникальный идентификатор
  /// </summary>
  public Guid Id { get; set; }

  /// <summary>
  /// Имя пользователя
  /// </summary>
  public string Username { get; set; } = string.Empty;

  /// <summary>
  /// Хешированный пароль
  /// </summary>
  public string PasswordHash { get; set; } = string.Empty;

  /// <summary>
  /// Роль пользователя
  /// </summary>
  public UserRole Role {  get; set; }

  /// <summary>
  /// Дата регистрации
  /// </summary>
  public DateTime CreatedAt { get; set; }
}
