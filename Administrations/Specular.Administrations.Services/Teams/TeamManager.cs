using NgSoftware.Specular.Administrations.Services.Contracts.Interfaces;
using NgSoftware.Specular.Administrations.Services.Contracts.Models.Teams;

namespace NgSoftware.Specular.Administrations.Services.Teams;

/// <inheritdoc cref="ITeamManager"/>
public class TeamManager : ITeamManager, IAdministrationsServiceAnchor
{
    Task<IReadOnlyCollection<TeamModel>> ITeamManager.GetTeamsByUserIdAsync(Guid userId, CancellationToken cancellationToken) => throw new NotImplementedException();

    Task<TeamModel> ITeamManager.CreateAsync(CreateTeamModel model, CancellationToken cancellationToken) => throw new NotImplementedException();

    Task<TeamModel> ITeamManager.UpdateAsync(UpdateTeamModel model, CancellationToken cancellationToken) => throw new NotImplementedException();

    Task ITeamManager.DeleteAsync(DeleteTeamModel model, CancellationToken cancellationToken) => throw new NotImplementedException();
}
