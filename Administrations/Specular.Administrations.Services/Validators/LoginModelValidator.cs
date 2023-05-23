using FluentValidation;
using NgSoftware.Specular.Administrations.Services.Contracts.Models.User;

namespace NgSoftware.Specular.Administrations.Services.Validators;

/// <summary>
/// Валидация <see cref="LoginModel"/>
/// </summary>
public class LoginModelValidator : AbstractValidator<LoginModel>
{
    /// <summary>
    /// Инициализирует новый экземпляр <see cref="LoginModelValidator"/>
    /// </summary>
    public LoginModelValidator()
    {
        RuleFor(x => x.Login).NotEmpty();
        RuleFor(x => x.Password).NotEmpty();
    }
}
