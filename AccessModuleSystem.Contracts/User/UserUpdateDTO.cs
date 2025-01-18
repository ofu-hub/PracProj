namespace AccessModuleSystem.Contracts.User;

/// <summary>
/// DTO для обновления информации о пользователе
/// </summary>
public class UserUpdateDTO
{
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
  /// Заблокирован
  /// </summary>
  public bool IsBlocked { get; set; }
}