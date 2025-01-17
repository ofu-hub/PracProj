using AccessModuleSystem.Client.Services;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using System.Text.Json;

namespace AccessModuleSystem.Client.Providers;

public class CustomAuthenticationStateProvider : AuthenticationStateProvider
{
  private readonly ITokenService tokenService;

  public CustomAuthenticationStateProvider(ITokenService tokenService)
  {
    this.tokenService = tokenService;
  }

  public void StateChanged()
  {
    NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
  }

  public override async Task<AuthenticationState> GetAuthenticationStateAsync()
  {
    Console.WriteLine("GetAuthenticationStateAsync");
    var tokenDTO = await tokenService.GetToken();

    if (tokenDTO is null)
    {
      Console.WriteLine("TokenDTO is null.");
      return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
    }
    else
    {
      Console.WriteLine($"TokenDTO found: {tokenDTO.AccessToken}");

      var identity = string.IsNullOrEmpty(tokenDTO.AccessToken) || tokenDTO.Expiration < DateTime.UtcNow
        ? new ClaimsIdentity()
        : new ClaimsIdentity(ParseClaimsFromJwt(tokenDTO.AccessToken), "jwt", ClaimTypes.Name, "role");

      return new AuthenticationState(new ClaimsPrincipal(identity));
    }
  }

  public async Task SignOutAsync()
  {
    // Логика выхода: удаление токена, сброс состояния и т.д.
    await Task.Run(() =>
    {
      tokenService.RemoveToken();

      NotifyAuthenticationStateChanged(Task.FromResult(GetAnonymousState()));
    });
  }

  private AuthenticationState GetAnonymousState()
  {
    var anonymousIdentity = new ClaimsIdentity();
    return new AuthenticationState(new ClaimsPrincipal(anonymousIdentity));
  }

  private static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
  {
    var payload = jwt.Split('.')[1];
    var jsonBytes = ParseBase64WithoutPadding(payload);
    var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

    if (keyValuePairs == null)
    {
      return Enumerable.Empty<Claim>();
    }

    return keyValuePairs
        .Where(kvp => kvp.Key != null && kvp.Value != null)
        .Select(kvp => new Claim(kvp.Key, kvp.Value.ToString() ?? string.Empty));
  }

  private static byte[] ParseBase64WithoutPadding(string base64)
  {
    if (String.IsNullOrWhiteSpace(base64)) return Array.Empty<byte>();
    try
    {
      string working = base64.Replace('-', '+').Replace('_', '/'); ;
      while (working.Length % 4 != 0)
      {
        working += '=';
      }
      return Convert.FromBase64String(working);
    }
    catch (Exception)
    {
      return Array.Empty<byte>();
    }
  }
}