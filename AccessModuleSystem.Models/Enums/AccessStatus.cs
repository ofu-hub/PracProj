﻿using System.ComponentModel.DataAnnotations;

namespace AccessModuleSystem.Models.Enums;

/// <summary>
/// Статус доступа
/// </summary>
public enum AccessStatus
{
  /// <summary>
  /// Предоставленный
  /// </summary>
  [Display(Name = "Предоставленный")] 
  Granted = 0,

  /// <summary>
  /// Отклонен
  /// </summary>
  [Display(Name = "Отклонен")] 
  Denied = 1
}
