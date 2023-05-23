using NgSoftware.Specular.Common.Core.Contracts;
using Serilog;
using Serilog.Events;

namespace NgSoftware.Specular.Common.Mvc.Implementations;

/// <inheritdoc />
internal class SpecularLogger : ISpecularLogger
{
    private readonly ILogger logger;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="SpecularLogger"/>
    /// </summary>
    private SpecularLogger(ILogger logger)
    {
        this.logger = logger;
    }

    /// <summary>
    /// Создаёт новый экземпляр <see cref="ISpecularLogger"/>
    /// </summary>
    static internal ISpecularLogger Create(ILogger logger)
        => new SpecularLogger(logger);

    void ISpecularLogger.Trace(string messageTemplate, params object[] propertyValues)
        => Write(LogEventLevel.Verbose, null, messageTemplate, propertyValues);

    void ISpecularLogger.Trace(string messageTemplate, Exception exception, params object[] propertyValues)
        => Write(LogEventLevel.Verbose, exception, messageTemplate, propertyValues);

    void ISpecularLogger.Debug(string messageTemplate, params object[] propertyValues)
        => Write(LogEventLevel.Debug, null, messageTemplate, propertyValues);

    void ISpecularLogger.Debug(string messageTemplate, Exception exception, params object[] propertyValues)
        => Write(LogEventLevel.Debug, exception, messageTemplate, propertyValues);

    void ISpecularLogger.Info(string messageTemplate, params object[] propertyValues)
        => Write(LogEventLevel.Information, null, messageTemplate, propertyValues);

    void ISpecularLogger.Info(string messageTemplate, Exception exception, params object[] propertyValues)
        => Write(LogEventLevel.Information, exception, messageTemplate, propertyValues);

    void ISpecularLogger.Warn(string messageTemplate, params object[] propertyValues)
        => Write(LogEventLevel.Warning, null, messageTemplate, propertyValues);

    void ISpecularLogger.Warn(string messageTemplate, Exception exception, params object[] propertyValues)
        => Write(LogEventLevel.Warning, exception, messageTemplate, propertyValues);

    void ISpecularLogger.Error(string messageTemplate, params object[] propertyValues)
        => Write(LogEventLevel.Error, null, messageTemplate, propertyValues);

    void ISpecularLogger.Error(string messageTemplate, Exception exception, params object[] propertyValues)
        => Write(LogEventLevel.Error, exception, messageTemplate, propertyValues);

    void ISpecularLogger.Fatal(string messageTemplate, params object[] propertyValues)
        => Write(LogEventLevel.Fatal, null, messageTemplate, propertyValues);

    void ISpecularLogger.Fatal(string messageTemplate, Exception exception, params object[] propertyValues)
        => Write(LogEventLevel.Fatal, exception, messageTemplate, propertyValues);

    private void Write(LogEventLevel level, Exception? exception, string messageTemplate, object[] propertyValues)
        => logger.Write(level, exception, messageTemplate, propertyValues);
}
