﻿using AccessModuleSystem.Models;
using AccessModuleSystem.Server.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace AccessModuleSystem.Server.Helpers;

/// <summary>
/// Класс для работы с Jwt
/// </summary>
public class JwtHelper
{
  private const int RefreshTokenLength = 64;
  private readonly IOptions<JwtOptions> _jwtOptions;
  private readonly ILogger<JwtHelper> _logger;

  /// <summary>
  /// Конструктор класса <see cref="JwtHelper"/>
  /// </summary>
  /// <param name="jwtOptions">Настройки jwt</param>
  /// <param name="logger">Логгер</param>
  public JwtHelper(IOptions<JwtOptions> jwtOptions, ILogger<JwtHelper> logger)
  {
    _jwtOptions = jwtOptions;
    _logger = logger;
  }

  /// <summary>
  /// Прочитать Jwt
  /// </summary>
  /// <param name="token">Jwt</param>
  /// <param name="claims">Данные пользователя</param>
  /// <param name="validTo">Дата валидности</param>
  /// <returns></returns>
  public bool ReadAccessToken(string token, out ClaimsPrincipal? claims, out DateTime validTo)
  {
    var tokenHandler = new JwtSecurityTokenHandler();
    var validations = new TokenValidationParameters
    {
      ValidateIssuerSigningKey = true,
      IssuerSigningKey = _jwtOptions.Value.GetSymmetricSecurityKey(),
      ValidateIssuer = true,
      ValidIssuer = _jwtOptions.Value.Issuer,
      ValidateAudience = true,
      ValidAudience = _jwtOptions.Value.Audience,
      ValidateLifetime = true
    };

    try
    {
      claims = tokenHandler.ValidateToken(token, validations, out var validatedToken);
      validTo = validatedToken.ValidTo;
      return true;
    }
    catch (SecurityTokenException ex)
    {
      _logger.LogWarning(message: ex.ToString());
    }

    claims = null;
    validTo = DateTime.MinValue;
    return false;
  }

  /// <summary>
  /// Создать токен доступа
  /// </summary>
  /// <param name="user">Пользователь</param>
  /// <param name="minutesValid">Время действия токена</param>
  /// <returns>Токен доступа</returns>
  public string CreateAccessToken(User user, int minutesValid)
{
    var claims = new[]
    {
        new Claim(ClaimTypes.Name, user.Name),
        new Claim(ClaimTypes.Surname, user.Surname),
        new Claim(ClaimTypes.Email, user.Email),
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(ClaimTypes.Role, user.Role.ToString())
    };

    var tokenDescriptor = new SecurityTokenDescriptor
    {
        Issuer = _jwtOptions.Value.Issuer,
        Audience = _jwtOptions.Value.Audience,
        IssuedAt = DateTime.UtcNow,
        NotBefore = DateTime.UtcNow,
        Expires = DateTime.UtcNow.AddMinutes(minutesValid),
        Subject = new ClaimsIdentity(claims), // Используем ClaimsIdentity
        SigningCredentials = new SigningCredentials(
            _jwtOptions.Value.GetSymmetricSecurityKey(),
            SecurityAlgorithms.HmacSha256Signature
        )
    };

    var tokenHandler = new JwtSecurityTokenHandler();
    var token = tokenHandler.CreateToken(tokenDescriptor);
    return tokenHandler.WriteToken(token);
}

  /// <summary>
  /// Создать токен обновления
  /// </summary>
  /// <returns>Токен обновления</returns>
  public static string CreateRefreshToken()
  {
    var token = RandomNumberGenerator.GetBytes(RefreshTokenLength);
    var bytes = Encoding.UTF8.GetBytes(Convert.ToBase64String(token));
    var hash = SHA256.HashData(bytes);
    return Convert.ToBase64String(hash);
  }
}