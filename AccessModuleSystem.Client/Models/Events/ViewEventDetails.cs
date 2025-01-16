using AccessModuleSystem.Client.Models.Enums;

namespace AccessModuleSystem.Client.Models.Events;

/// <summary>
/// Модель для отображения подробной информации о событии доступа
/// </summary>
public class ViewEventDetails
{
  // Событие доступа
  public Guid EventId { get; set; }
  public DateTime Timestamp { get; set; }
  public AccessType AccessType { get; set; }
  public AccessStatus AccessStatus { get; set; }

  // Транспортное средство
  public Guid VehicleId { get; set; }
  public string LicensePlate { get; set; } = string.Empty;
  public string OwnerName { get; set; } = string.Empty;
  public PermissionStatus VehicleStatus { get; set; }
  public DateTime VehicleCreatedAt { get; set; }

  // Камера
  public Guid CameraId { get; set; }
  public string CameraLocation { get; set; } = string.Empty;
  public CameraStatus CameraStatus { get; set; }
}