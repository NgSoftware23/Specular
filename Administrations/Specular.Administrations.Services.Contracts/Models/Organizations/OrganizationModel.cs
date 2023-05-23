﻿namespace NgSoftware.Specular.Administrations.Services.Contracts.Models.Organizations;

/// <summary>
/// Модель организации
/// </summary>
public class OrganizationModel
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Имя
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Описание
    /// </summary>
    public string Description { get; set; } = string.Empty;
}
