using AutoMapper;
using NgSoftware.Specular.Administrations.Api.DI;
using NgSoftware.Specular.Api.Infrastructures;
using NgSoftware.Specular.Common.Mvc.Extensions;
using Specular.Common.Core.Implementations;
using Module = NgSoftware.Specular.Common.Mvc.Module;

namespace NgSoftware.Specular.Api.DI;

/// <inheritdoc />
public class ApiModule : Module
{
    /// <inheritdoc />
    protected override void Load(IServiceCollection services)
    {
        services.RegisterAsImplementedInterfaces<DateTimeProvider>(ServiceLifetime.Singleton);
        services.RegisterAsImplementedInterfaces<ApiIdentityProvider>(ServiceLifetime.Scoped);
        services.RegisterAsImplementedInterfaces<DbWriterContext>(ServiceLifetime.Scoped);
        services.RegisterAsImplementedInterfaces<SpecularConfiguration>(ServiceLifetime.Singleton);
        services.AddHttpContextAccessor();

        services.RegisterModule<AdministrationModule>();

        RegisterAutoMapper(services);
    }

    private static void RegisterAutoMapper(IServiceCollection services)
    {
        services.AddSingleton<IMapper>(provider =>
        {
            var profiles = provider.GetServices<Profile>();
            var mapperConfig = new MapperConfiguration(mc =>
            {
                foreach (var profile in profiles)
                {
                    mc.AddProfile(profile);
                }
            });
            var mapper = mapperConfig.CreateMapper();
            return mapper;
        });
    }
}
