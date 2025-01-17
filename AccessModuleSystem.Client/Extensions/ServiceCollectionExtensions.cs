namespace AccessModuleSystem.Client.Extensions;

public static class ServiceCollectionExtensions
{
  public static IServiceCollection AddPolicies(this IServiceCollection services)
  {
    services.AddAuthorizationCore(config =>
    {
      config.AddPolicy("Admin", policy => policy.RequireClaim("role", "Admin"));
    });

    return services;
  }
}