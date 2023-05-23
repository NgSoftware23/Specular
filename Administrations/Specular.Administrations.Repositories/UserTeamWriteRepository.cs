using NgSoftware.Specular.Administrations.Entities;
using NgSoftware.Specular.Administrations.Repositories.Contracts;
using NgSoftware.Specular.Common.Repositories;
using NgSoftware.Specular.Context.Contracts;

namespace NgSoftware.Specular.Administrations.Repositories;

/// <inheritdoc cref="IUserTeamWriteRepository"/>
public class UserTeamWriteRepository : BaseWriteRepository<UserTeam>,
    IUserTeamWriteRepository,
    IAdministrationRepositoryAnchor
{
    /// <summary>
    /// Инициализирует новый экземпляр <see cref="UserTeamWriteRepository"/>
    /// </summary>
    public UserTeamWriteRepository(IDbWriterContext writerContext) : base(writerContext)
    {

    }
}
