using AccessModuleSystem.Contracts.Auth;

namespace AccessModuleSystem.Client.Services;

public interface ITokenService
{
  Task<GetTokenDto?> GetToken();
  Task RemoveToken();
  Task SetToken(GetTokenDto tokenDTO);
}
