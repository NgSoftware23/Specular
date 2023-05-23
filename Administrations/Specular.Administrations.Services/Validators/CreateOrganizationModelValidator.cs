using FluentValidation;
using NgSoftware.Specular.Administrations.Repositories.Contracts;
using NgSoftware.Specular.Administrations.Services.Contracts.Models.Organizations;
using NgSoftware.Specular.Administrations.Services.Resources;

namespace NgSoftware.Specular.Administrations.Services.Validators;

/// <summary>
/// Валидация <see cref="CreateOrganizationModel"/>
/// </summary>
public class CreateOrganizationModelValidator :  AbstractValidator<CreateOrganizationModel>
{
    /// <summary>
    /// Инициализирует новый экземпляр <see cref="CreateOrganizationModelValidator"/>
    /// </summary>
    public CreateOrganizationModelValidator(IOrganizationReadRepository organizationReadRepository)
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Name)
            .MustAsync(async (name, cancellationToken) =>
            {
                var exists = await organizationReadRepository.IsActiveNameExistsAsync(name, cancellationToken);
                return !exists;
            })
            .WithMessage(ErrorMessages.OrganizationAlreadyExist);
    }
}
