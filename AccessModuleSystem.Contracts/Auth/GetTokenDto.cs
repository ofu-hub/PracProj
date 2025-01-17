namespace AccessModuleSystem.Contracts.Auth;

/// <summary>
/// Модель получения токена
/// </summary>
public class GetTokenDto
{
  /// <summary>
  /// Токен
  /// </summary>
  public string? AccessToken { get; set; }

  /// <summary>
  /// Время
  /// </summary>
  public DateTime Expiration { get; set; }
}
