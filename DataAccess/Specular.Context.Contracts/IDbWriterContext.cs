using NgSoftware.Specular.Common.Core.Contracts;

namespace NgSoftware.Specular.Context.Contracts;

/// <summary>
/// Определяет контекст репозитория записи сущностей
/// </summary>
public interface IDbWriterContext
{
    /// <inheritdoc cref="IWriter"/>
    IWriter Writer { get; }

    /// <inheritdoc cref="IUnitOfWork"/>
    IUnitOfWork UnitOfWork { get; }

    /// <inheritdoc cref="IDateTimeProvider"/>
    IDateTimeProvider DateTimeProvider { get; }

    /// <inheritdoc cref="IIdentityProvider"/>
    IIdentityProvider IdentityProvider { get; }
}
