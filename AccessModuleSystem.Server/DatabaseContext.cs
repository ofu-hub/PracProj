using Microsoft.EntityFrameworkCore;

namespace AccessModuleSystem.Server;

public class DatabaseContext : DbContext
{
  public DatabaseContext() { }
  public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
  {
    if (Database.GetPendingMigrations().Any())
      Database.Migrate();
  }
}
