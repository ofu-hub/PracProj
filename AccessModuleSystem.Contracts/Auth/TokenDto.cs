namespace AccessModuleSystem.Contracts.Auth;

/// <summary>
/// Модель токена
/// </summary>
public class TokenDto
{
  /// <summary>
  /// Access-токен
  /// </summary>
  public string AccessToken { get; set; } = string.Empty;

  /// <summary>
  /// Refresh-токен
  /// </summary>
  public string RefreshToken { get; set; } = string.Empty;
}