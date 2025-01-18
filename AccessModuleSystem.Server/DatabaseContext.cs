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
  public DbSet<Vehicle> Vehicles { get; set; } = null!;
  public DbSet<Camera> Cameras { get; set; } = null!;
  public DbSet<AccessEvent> AccessEvents { get; set; } = null!;
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
      entity.Property(e => e.Name).IsRequired();
      entity.Property(e => e.Surname).IsRequired();
      entity.Property(e => e.Patronymic).IsRequired(false);
      entity.Property(e => e.Email).IsRequired();
      entity.Property(e => e.Role).IsRequired();
      entity.Property(e => e.IsBlocked).IsRequired();
      entity.Property(e => e.PasswordHash).IsRequired();
      entity.Property(e => e.CreatedAt).IsRequired();

      entity.Property(e => e.VehicleId).IsRequired(false);
      entity.HasOne(e => e.Vehicle).WithOne().HasForeignKey<User>(e => e.VehicleId).IsRequired(false);

      entity.Property(e => e.RefreshToken).IsRequired(false);
      entity.Property(e => e.RefreshTokenExpires).IsRequired(false);

      entity.HasData(
        new User
        {
          Id = Guid.Parse("d1b0f2cc-79b3-4b69-928c-8b263f2ab9c4"),
          CreatedAt = DateTime.SpecifyKind(DateTime.Parse("2023-06-15T20:37:19.000000Z"), DateTimeKind.Utc),
          Role = UserRole.Admin,
          Username = "admin",
          Email = "admin@ams.ru",
          Name = "Павел",
          Surname = "Маркелов",
          PasswordHash = "admin",
        },
        new User
        {
          Id = Guid.Parse("d1b0f4cc-79b3-4b69-988c-8b263f2ab9c4"),
          CreatedAt = DateTime.SpecifyKind(DateTime.Parse("2023-05-15T20:37:19.000000Z"), DateTimeKind.Utc),
          Role = UserRole.User,
          Username = "user",
          Email = "user@ams.ru",
          Name = "Иван",
          Surname = "Иванов",
          PasswordHash = "user"
        }
      );
    });

    modelBuilder.Entity<Vehicle>(entity =>
    {
      entity.HasKey(e => e.Id);

      entity.Property(e => e.LicensePlate).IsRequired();
      entity.Property(e => e.OwnerName).IsRequired();
      entity.Property(e => e.Status).IsRequired();
      entity.Property(e => e.CreatedAt).IsRequired();
      entity.Property(e => e.DeactivationAt).IsRequired(false);

      entity.Property(e => e.UserId).IsRequired(false);
      entity.HasOne(e => e.User).WithOne().HasForeignKey<Vehicle>(e => e.UserId).IsRequired(false);

      entity.HasData(
        new Vehicle
        {
          Id = Guid.Parse("d1b0f2cc-14b3-4b69-928c-8b263f3ab9c4"),
          CreatedAt = DateTime.SpecifyKind(DateTime.Parse("2023-05-15T20:37:19.000000Z"), DateTimeKind.Utc),
          LicensePlate = "A726BC 30 RUS",
          OwnerName = "Петров Петр",
          Status = PermissionStatus.Active,
        },
        new Vehicle
        {
          Id = Guid.Parse("d1b0f2cc-12b3-4b69-928c-8b263f3ab9c4"),
          CreatedAt = DateTime.SpecifyKind(DateTime.Parse("2023-05-15T20:37:19.000000Z"), DateTimeKind.Utc),
          LicensePlate = "A123BC 30 RUS",
          OwnerName = "Иванов Иван Иванович",
          Status = PermissionStatus.Active,
        }
      );
    });
  }
}
