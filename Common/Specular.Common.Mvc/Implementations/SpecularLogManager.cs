using NgSoftware.Specular.Common.Core.Contracts;
using Serilog;

namespace NgSoftware.Specular.Common.Mvc.Implementations;

/// <inheritdoc />
public class SpecularLogManager : ISpecularLogManager
{
    private readonly ILogger rootLogger;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="SpecularLogManager"/>
    /// </summary>
    public SpecularLogManager(ILogger rootLogger)
    {
        this.rootLogger = rootLogger;
    }

    ISpecularLogger ISpecularLogManager.GetLogger<T>()
        => SpecularLogger.Create(GetLoggerInternal(typeof(T).FullName, Array.Empty<string>()));

    ISpecularLogger ISpecularLogManager.GetLogger<T>(params string[] tags)
        => SpecularLogger.Create(GetLoggerInternal(typeof(T).FullName, tags));

    private ILogger GetLoggerInternal(string? loggerName, IEnumerable<string> tags)
    {
        var result = rootLogger.ForContext("Logger", loggerName ?? "unknown");

        if (tags.Any())
        {
            result = result.ForContext("Tags", string.Join(", ", tags));
        }

        return result;
    }
}
