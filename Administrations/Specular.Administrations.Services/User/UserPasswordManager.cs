using NgSoftware.Specular.Administrations.Services.Contracts.Interfaces;
using NgSoftware.Specular.Administrations.Services.Contracts.Models.Password;
using NgSoftware.Specular.Administrations.Services.Contracts.Models.User;

namespace NgSoftware.Specular.Administrations.Services.User;

/// <inheritdoc cref="IUserPasswordManager"/>
public class UserPasswordManager : IUserPasswordManager, IAdministrationsServiceAnchor
{
    Task<UserLoggedModel> IUserPasswordManager.ChangePasswordAsync(ChangePasswordModel model, CancellationToken cancellationToken) => throw new NotImplementedException();

    Task IUserPasswordManager.SetTemporaryPasswordAsync(SetTemporaryPasswordModel model, CancellationToken cancellationToken) => throw new NotImplementedException();

    Task<UserLoggedModel> IUserPasswordManager.ChangeTemporaryPasswordAsync(ChangeTemporaryPasswordModel model, CancellationToken cancellationToken) => throw new NotImplementedException();
}
