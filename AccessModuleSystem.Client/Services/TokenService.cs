using AccessModuleSystem.Contracts.Auth;
using Blazored.LocalStorage;
using System.Threading.Tasks;

namespace AccessModuleSystem.Client.Services;

public class TokenService : ITokenService
{
  private readonly ILocalStorageService localStorageService;

  public TokenService(ILocalStorageService localStorageService)
  {
    this.localStorageService = localStorageService;
  }

  public async Task SetToken(GetTokenDto tokenDTO)
  {
    await localStorageService.SetItemAsync("token", tokenDTO);
  }

  public async Task<GetTokenDto?> GetToken()
  {
    var token = await localStorageService.GetItemAsync<GetTokenDto>("token");
    if (token == null)
    {
      Console.WriteLine("Token not found in local storage.");
      return null;
    }
    return token;
  }

  public async Task RemoveToken()
  {
    await localStorageService.RemoveItemAsync("token");
  }
}
