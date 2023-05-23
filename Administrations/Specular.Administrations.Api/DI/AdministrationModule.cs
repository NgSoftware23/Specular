using Microsoft.Extensions.DependencyInjection;
using NgSoftware.Specular.Administrations.Api.AutoMappers;
using NgSoftware.Specular.Administrations.Repositories;
using NgSoftware.Specular.Administrations.Services;
using NgSoftware.Specular.Administrations.Services.AutoMappers;
using NgSoftware.Specular.Common.Mvc;
using NgSoftware.Specular.Common.Mvc.Extensions;

namespace NgSoftware.Specular.Administrations.Api.DI;

/// <summary>
/// Модуль регистрации зависимостей для администрирования
/// </summary>
public class AdministrationModule : Module
{
    /// <inheritdoc />
    protected override void Load(IServiceCollection services)
    {
        services.RegisterAssemblyInterfacesAssignableTo<IAdministrationRepositoryAnchor>(ServiceLifetime.Scoped);
        services.RegisterAssemblyInterfacesAssignableTo<IAdministrationsServiceAnchor>(ServiceLifetime.Scoped);
        services.RegisterAutoMapperProfile<AdministrationServiceProfile>();
        services.RegisterAutoMapperProfile<AdministrationMapperProfile>();
    }
}
