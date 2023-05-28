using FluentValidation;
using NgSoftware.Specular.Administrations.Repositories.Contracts;
using NgSoftware.Specular.Administrations.Services.Contracts.Models.Organizations;
using NgSoftware.Specular.Administrations.Services.Resources;

namespace NgSoftware.Specular.Administrations.Services.Validators;

/// <summary>
/// Валидация <see cref="UpdateOrganizationModel"/>
/// </summary>
public class UpdateOrganizationModelValidator : AbstractValidator<UpdateOrganizationModel>
{
    /// <summary>
    /// Инициализирует новый экземпляр <see cref="UpdateOrganizationModelValidator"/>
    /// </summary>
    public UpdateOrganizationModelValidator(IOrganizationReadRepository organizationReadRepository)
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x)
            .MustAsync(async (request, cancellationToken) =>
            {
                var organization = await organizationReadRepository.GetActiveByNameAsync(request.Name.Trim().ToLower(), cancellationToken);
                var result = organization == null || organization.Id == request.Id;
                return result;
            })
            .WithName(x => nameof(x.Name))
            .WithMessage(ErrorMessages.OrganizationAlreadyExist);
    }
}
