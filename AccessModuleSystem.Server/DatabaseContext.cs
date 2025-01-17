using AccessModuleSystem.Models;
using AccessModuleSystem.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace AccessModuleSystem.Server;

/// <summary>
/// Контекст базы данных
/// </summary>
public class DatabaseContext : DbContext
{
  #region Tables
  public DbSet<User> Users { get; set; } = null!;
  #endregion

  /// <summary>
  /// Конструктор по умолчанию
  /// </summary>
  public DatabaseContext() { }

  /// <summary>
  /// Конструктор с опциями
  /// </summary>
  /// <param name="options"></param>
  public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
  {
    if (Database.GetPendingMigrations().Any())
      Database.Migrate();
  }

  /// <summary>
  /// Создание моделей
  /// </summary>
  /// <param name="modelBuilder"></param>
  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.Entity<User>(entity =>
    {
      entity.HasKey(e => e.Id);

      entity.Property(e => e.Username).IsRequired();
      entity.Property(e => e.Role).IsRequired();
      entity.Property(e => e.PasswordHash).IsRequired();
      entity.Property(e => e.CreatedAt).IsRequired();

      entity.Property(e => e.RefreshToken).IsRequired(false);
      entity.Property(e => e.RefreshTokenExpires).IsRequired(false);

      entity.HasData(
        new User
        {
          Id = Guid.Parse("d1b0f2cc-79b3-4b69-928c-8b263f2ab9c4"),
          CreatedAt = DateTime.SpecifyKind(DateTime.Parse("2023-06-15T20:37:19.000000Z"), DateTimeKind.Utc),
          Role = UserRole.Admin,
          Username = "admin",
          PasswordHash = "admin",
        },
        new User
        {
          Id = Guid.Parse("d1b0f4cc-79b3-4b69-988c-8b263f2ab9c4"),
          CreatedAt = DateTime.SpecifyKind(DateTime.Parse("2023-05-15T20:37:19.000000Z"), DateTimeKind.Utc),
          Role = UserRole.User,
          Username = "user",
          PasswordHash = "user"
        }
      );
    });
  }
}
