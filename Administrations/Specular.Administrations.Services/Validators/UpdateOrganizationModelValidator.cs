using FluentValidation;
using NgSoftware.Specular.Administrations.Services.Contracts.Models.Organizations;

namespace NgSoftware.Specular.Administrations.Services.Validators;

/// <summary>
/// Валидация <see cref="UpdateOrganizationModel"/>
/// </summary>
public class UpdateOrganizationModelValidator : AbstractValidator<UpdateOrganizationModel>
{
    /// <summary>
    /// Инициализирует новый экземпляр <see cref="UpdateOrganizationModelValidator"/>
    /// </summary>
    public UpdateOrganizationModelValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Description);
    }
}
