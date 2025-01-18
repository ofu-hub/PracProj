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
      entity.HasKey(u => u.Id);

      entity.Property(u => u.Username).IsRequired();
      entity.Property(u => u.Name).IsRequired();
      entity.Property(u => u.Surname).IsRequired();
      entity.Property(u => u.Patronymic).IsRequired(false);
      entity.Property(u => u.Email).IsRequired();
      entity.Property(u => u.Role).IsRequired();
      entity.Property(u => u.IsBlocked).IsRequired();
      entity.Property(u => u.PasswordHash).IsRequired();
      entity.Property(u => u.CreatedAt).IsRequired();

      entity.Property(u => u.VehicleId).IsRequired(false);
      entity.HasOne(u => u.Vehicle)
      .WithOne()
      .HasForeignKey<User>(u => u.VehicleId)
      .IsRequired(false);

      entity.Property(u => u.RefreshToken).IsRequired(false);
      entity.Property(u => u.RefreshTokenExpires).IsRequired(false);

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
      entity.HasKey(v => v.Id);

      entity.Property(v => v.LicensePlate).IsRequired();
      entity.Property(v => v.OwnerName).IsRequired();
      entity.Property(v => v.Status).IsRequired();
      entity.Property(v => v.CreatedAt).IsRequired();
      entity.Property(v => v.DeactivationAt).IsRequired(false);

      entity.Property(v => v.UserId).IsRequired(false);
      entity.HasOne(v => v.User)
      .WithOne()
      .HasForeignKey<Vehicle>(v => v.UserId)
      .IsRequired(false);

      entity.HasData(
        new Vehicle
        {
          Id = Guid.Parse("15e9434f-0499-4b67-9855-8b82379bf458"),
          CreatedAt = DateTime.SpecifyKind(DateTime.Parse("2023-05-15T20:37:19.000000Z"), DateTimeKind.Utc),
          LicensePlate = "A726BC 30 RUS",
          OwnerName = "Петров Петр",
          Status = PermissionStatus.Active,
        },
        new Vehicle
        {
          Id = Guid.Parse("5b037b65-19f1-402a-ad5b-9779ef098b19"),
          CreatedAt = DateTime.SpecifyKind(DateTime.Parse("2023-05-15T20:37:19.000000Z"), DateTimeKind.Utc),
          LicensePlate = "A123BC 30 RUS",
          OwnerName = "Иванов Иван Иванович",
          Status = PermissionStatus.Active,
        }
      );
    });

    modelBuilder.Entity<AccessEvent>(entity =>
    {
      entity.HasKey(a => a.Id);

      entity.Property(a => a.AccessType).IsRequired();
      entity.Property(a => a.Status).IsRequired();
      entity.Property(a => a.Timestamp).IsRequired();

      entity.HasOne(a => a.Vehicle)
      .WithMany()
      .HasForeignKey(a => a.VehicleId);

      entity.HasOne(a => a.Camera)
      .WithMany()
      .HasForeignKey(a => a.CameraId);

      entity.HasData(
        new AccessEvent
        {
          Id = Guid.Parse("b94ff912-f540-41ad-bca5-a97b6e57f21c"),
          Timestamp = DateTime.SpecifyKind(DateTime.Parse("2023-05-15T20:37:19.000000Z"), DateTimeKind.Utc),
          AccessType = AccessType.Entry,
          Status = AccessStatus.Granted,
          CameraId = Guid.Parse("31376f03-ffed-427b-a5cf-e7a04105d689"),
          VehicleId = Guid.Parse("15e9434f-0499-4b67-9855-8b82379bf458")

        },
        new AccessEvent
        {
          Id = Guid.Parse("31e67b52-4b16-4bdc-ac3e-3c33b8040a29"),
          Timestamp = DateTime.SpecifyKind(DateTime.Parse("2023-05-15T20:37:19.000000Z"), DateTimeKind.Utc),
          AccessType = AccessType.Entry,
          Status = AccessStatus.Granted,
          CameraId = Guid.Parse("dbc4b6ba-d49f-4785-8ba0-14516620ae66"),
          VehicleId = Guid.Parse("5b037b65-19f1-402a-ad5b-9779ef098b19")
        }
      );
    });

    modelBuilder.Entity<Camera>(entity =>
    {
      entity.HasKey(c => c.Id);

      entity.Property(c => c.Location).IsRequired();
      entity.Property(c => c.Status).IsRequired();

      entity.HasData(
        new Camera
        {
          Id = Guid.Parse("31376f03-ffed-427b-a5cf-e7a04105d689"),
          Location = "ул. Пушкина, д.1, к.1",
          Status = CameraStatus.Maintenance
        },
        new Camera
        {
          Id = Guid.Parse("dbc4b6ba-d49f-4785-8ba0-14516620ae66"),
          Location = "ул. Пушкина, д.2, к.1",
          Status = CameraStatus.Active

        }
      );
    });
  }
}
