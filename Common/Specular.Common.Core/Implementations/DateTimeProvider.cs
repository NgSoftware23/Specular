using NgSoftware.Specular.Common.Core.Contracts;

namespace Specular.Common.Core.Implementations;

/// <inheritdoc cref="IDateTimeProvider"/>
public class DateTimeProvider : IDateTimeProvider
{
    DateTimeOffset IDateTimeProvider.UtcNow => DateTimeOffset.UtcNow;
}
