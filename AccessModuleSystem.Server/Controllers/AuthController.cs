using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using AccessModuleSystem.Contracts.Auth;
using AccessModuleSystem.Server.Helpers;
using AccessModuleSystem.Server.Options;
using Microsoft.EntityFrameworkCore;

namespace AccessModuleSystem.Server.Controllers;

/// <summary>
/// Контроллер работы с авторизацией
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
  private readonly DatabaseContext _context;
  private readonly IOptions<JwtOptions> _jwtOptions;
  private readonly JwtHelper _jwtHelper;
  private readonly ILogger<AuthController> _logger;

  /// <summary>
  /// Конструктор класса <see cref="AuthController"/>
  /// </summary>
  /// <param name="context">Контекст базы данных</param>
  /// <param name="jwtOptions">Настройки Jwt-токена</param>
  /// <param name="jwtHelper">Класс-помощник работы с Jwt</param>
  public AuthController(DatabaseContext context, IOptions<JwtOptions> jwtOptions,
      JwtHelper jwtHelper, ILogger<AuthController> logger)
  {
    _context = context;
    _jwtOptions = jwtOptions;
    _jwtHelper = jwtHelper;
    _logger = logger;
  }

  /// <summary>
  /// Авторизовать пользователя
  /// </summary>
  /// <param name="authDto">Данные по пользователю</param>
  /// <returns>Токены</returns>
  [HttpPost]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  public async Task<IActionResult> Auth([FromBody] AuthDto authDto)
  {
    var user = await _context.Users.FirstOrDefaultAsync(item => item.Username == authDto.Username);
    if (user is null) return NotFound("Пользователь не найден!");

    // todo: Hash password
    if (user.PasswordHash != authDto.Password) return Conflict("Ошибка! Неверный пароль!");

    user.RefreshToken = JwtHelper.CreateRefreshToken();
    user.RefreshTokenExpires = DateTime.UtcNow.AddSeconds(_jwtOptions.Value.RefreshTokenLifetime);

    _context.Users.Update(user);
    await _context.SaveChangesAsync();

    var tokens = new TokenDto
    {
      AccessToken = _jwtHelper.CreateAccessToken(user, _jwtOptions.Value.AccessTokenLifetime),
      RefreshToken = user.RefreshToken,
    };

    return Ok(new LoginResultDto
    {
      Succeeded = true,
      Token = new GetTokenDto
      {
        AccessToken = tokens.AccessToken,
        Expiration = (DateTime)user.RefreshTokenExpires
      }
    });
  }

  /// <summary>
  /// Обновляет токены с использованием валидного токена обновления. 
  /// </summary>
  /// <param name="refreshTokensDto">Данные токена обновления</param>
  /// <response code="200">Токены успешно обновлены</response>
  /// <response code="400">Передан некорректный токен обновления</response>
  /// <response code="401">Токен обновления устарел</response>
  /// <response code="500">Ошибка сервера</response>
  [HttpPost("refresh")]
  [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TokenDto))]
  [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(RefreshTokensDto))]
  [ProducesResponseType(StatusCodes.Status401Unauthorized)]
  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
  public async Task<IActionResult> RefreshTokens([FromBody] RefreshTokensDto refreshTokensDto)
  {
    if (string.IsNullOrEmpty(refreshTokensDto.RefreshToken))
      return BadRequest($"Поле {nameof(refreshTokensDto.RefreshToken)} не может быть пустым");

    var user = _context.Users.FirstOrDefault(c => c.RefreshToken == refreshTokensDto.RefreshToken);
    if (user is null)
      return NotFound($"Пользователь с токеном обновления {refreshTokensDto.RefreshToken} не найден");

    if (user.RefreshTokenExpires < DateTime.UtcNow)
      return Unauthorized($"Токен обновления устарел");

    user.RefreshToken = JwtHelper.CreateRefreshToken();
    user.RefreshTokenExpires = DateTime.UtcNow.AddMinutes(_jwtOptions.Value.RefreshTokenLifetime);
    _context.Users.Update(user);
    await _context.SaveChangesAsync();

    var tokens = new TokenDto
    {
      AccessToken = _jwtHelper.CreateAccessToken(user, _jwtOptions.Value.AccessTokenLifetime),
      RefreshToken = user.RefreshToken,
    };
    return Ok(tokens);
  }
}