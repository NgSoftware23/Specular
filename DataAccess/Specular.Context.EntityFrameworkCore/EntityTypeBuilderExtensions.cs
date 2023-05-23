using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NgSoftware.Specular.Context.Entities.Contracts.Interfaces;
using NgSoftware.Specular.Context.Entities.Contracts.Models;

namespace NgSoftware.Specular.Context.EntityFrameworkCore;

/// <summary>
/// Методы расширения для <see cref="EntityTypeBuilder"/>
/// </summary>
public static class EntityTypeBuilderExtensions
{
    /// <summary>
    /// Задаёт конфигурацию свойст аудита для сущности <inheritdoc cref="BaseAuditEntity"/>
    /// </summary>
    public static void PropertyAuditConfiguration<T>(this EntityTypeBuilder<T> builder)
        where T : BaseAuditEntity
    {
        builder.Property(x => x.CreatedAt).IsRequired();
        builder.Property(x => x.CreatedBy).IsRequired().HasMaxLength(200);
        builder.Property(x => x.UpdatedAt).IsRequired();
        builder.Property(x => x.UpdatedBy).IsRequired().HasMaxLength(200);
    }

    /// <summary>
    /// Задаёт конфигурацию ключа для идентификатора <see cref="IEntityWithId.Id"/>
    /// </summary>
    public static void HasIdAsKey<T>(this EntityTypeBuilder<T> builder)
        where T : class, IEntityWithId
        => builder.HasKey(x => x.Id);
}
