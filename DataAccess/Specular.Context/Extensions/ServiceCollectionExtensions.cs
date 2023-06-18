using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace NgSoftware.Specular.Context.Extensions;

/// <summary>
/// Методы расширения для <see cref="IServiceCollection"/>
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Добавляет контекст данных
    /// </summary>
    public static void AddSpecularContext(this IServiceCollection serviceCollection)
    {
        serviceCollection.TryAddSingleton<DbContextOptions<SpecularContext>>(provider =>
        {
            var configuration = provider.GetRequiredService<ISpecularContextConfiguration>();
            var dbContextOptions = new DbContextOptions<SpecularContext>(new Dictionary<Type, IDbContextOptionsExtension>());
            var optionsBuilder = new DbContextOptionsBuilder<SpecularContext>(dbContextOptions)
                .UseApplicationServiceProvider(provider)
                .UseNpgsql(configuration.ConnectionString);

            return optionsBuilder.Options;
        });

        serviceCollection.TryAddSingleton<DbContextOptions>(provider
            => provider.GetRequiredService<DbContextOptions<SpecularContext>>());

        serviceCollection.TryAddScoped<SpecularContext>();
        var interfaces = typeof(SpecularContext).GetTypeInfo()
            .ImplementedInterfaces
            .Where(i => i != typeof(IDisposable) && (i.IsPublic));

        foreach (Type interfaceType in interfaces)
        {
            serviceCollection.TryAdd(new ServiceDescriptor(interfaceType,
                provider => provider.GetRequiredService<SpecularContext>(),
                ServiceLifetime.Scoped));
        }
    }
}
