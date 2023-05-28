using FluentValidation.Results;
using NgSoftware.Specular.Administrations.Repositories.Contracts;
using NgSoftware.Specular.Administrations.Services.Contracts.Exceptions;
using NgSoftware.Specular.Administrations.Services.Contracts.Interfaces;
using NgSoftware.Specular.Administrations.Services.Validators;
using NgSoftware.Specular.Common.Core.Contracts.Models;
using Specular.Common.Core.Implementations;

namespace NgSoftware.Specular.Administrations.Services;

/// <inheritdoc cref="IAdministrationValidateService"/>
public class AdministrationValidateService : ValidateService,
    IAdministrationValidateService,
    IAdministrationsServiceAnchor
{
    /// <summary>
    /// Инициализирует новый экземпляр <see cref="AdministrationValidateService"/>
    /// </summary>
    public AdministrationValidateService(IUserReadRepository userReadRepository,
        IOrganizationReadRepository organizationReadRepository)
    {
        Register<CreateUserModelValidator>(userReadRepository);
        Register<LoginModelValidator>();
        Register<CreateOrganizationModelValidator>(organizationReadRepository);
        Register<UpdateOrganizationModelValidator>(organizationReadRepository);
    }

    /// <inheritdoc cref="ValidateService.ErrorsHandle"/>
    protected override Task ErrorsHandle(IEnumerable<ValidationFailure> validationFailures)
    {
        var errors = validationFailures
            .Select(x => InvalidateItemModel.New(x.PropertyName, x.ErrorMessage))
            .ToList();
        throw new AdministrationValidationException(errors);
    }
}
