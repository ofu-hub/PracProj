﻿using AccessModuleSystem.Contracts.Vehicle;
using AccessModuleSystem.Models.Enums;

namespace AccessModuleSystem.Contracts.User;

/// <summary>
/// DTO для детальной информации о пользователе
/// </summary>
public class UserDetailDTO
{
  /// <summary>
  /// Уникальный идентификатор
  /// </summary>
  public Guid Id { get; set; }

  /// <summary>
  /// Имя пользователя
  /// </summary>
  public string Username { get; set; } = string.Empty;

  /// <summary>
  /// Имя
  /// </summary>
  public string Name { get; set; } = string.Empty;

  /// <summary>
  /// Фамилия
  /// </summary>
  public string Surname { get; set; } = string.Empty;

  /// <summary>
  /// Отчество
  /// </summary>
  public string? Patronymic { get; set; }

  /// <summary>
  /// Почтовый электронный адрес
  /// </summary>
  public string Email { get; set; } = string.Empty;

  /// <summary>
  /// Роль пользователя
  /// </summary>
  public UserRole Role { get; set; }

  /// <summary>
  /// Заблокирован
  /// </summary>
  public bool IsBlocked { get; set; } = false;

  /// <summary>
  /// Дата регистрации
  /// </summary>
  public DateTime CreatedAt { get; set; }

  /// <summary>
  /// Транспортное средство
  /// </summary>
  public VehicleReadDTO? Vehicle { get; set; }
}
