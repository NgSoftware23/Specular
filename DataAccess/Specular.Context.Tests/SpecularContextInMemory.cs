using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using NgSoftware.Specular.Context.Contracts;

namespace NgSoftware.Specular.Context.Tests;

/// <summary>
/// Класс <see cref="SpecularContext"/> для тестов с базой в памяти. Один контекст на тест
/// </summary>
public class SpecularContextInMemory : IDisposable
{
    /// <summary>
    /// Контекст <see cref="SpecularContext"/>
    /// </summary>
    protected SpecularContext Context { get; }

    /// <inheritdoc cref="IDbWriterContext"/>
    protected IDbWriterContext WriterContext => new TestWriterContext(Context, Context);

    /// <inheritdoc cref="IUnitOfWork"/>
    protected IUnitOfWork UnitOfWork => Context;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="SpecularContextInMemory"/>
    /// </summary>
    protected SpecularContextInMemory()
    {
        var optionsBuilder = new DbContextOptionsBuilder<SpecularContext>()
            .UseInMemoryDatabase($"MoneronTests{Guid.NewGuid()}")
            .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning));
        Context = new SpecularContext(optionsBuilder.Options);
    }

    /// <inheritdoc cref="IDisposable"/>
    public void Dispose()
    {
        try
        {
            Context.Database.EnsureDeletedAsync().Wait();
            Context.Dispose();
        }
        catch (ObjectDisposedException ex)
        {
            Trace.TraceError(ex.Message);
        }
    }
}
