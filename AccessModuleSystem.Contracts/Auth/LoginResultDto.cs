namespace AccessModuleSystem.Contracts.Auth;

/// <summary>
/// Модель результата входа в систему
/// </summary>
public class LoginResultDto
{
  /// <summary>
  /// Результат
  /// </summary>
  public bool Succeeded { get; set; }

  /// <summary>
  /// Сообщение сервера
  /// </summary>
  public string? Message { get; set; }

  /// <summary>
  /// Токен
  /// </summary>
  public GetTokenDto Token { get; set; } = null!;
}
