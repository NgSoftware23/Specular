using NgSoftware.Specular.Administrations.Entities;
using NgSoftware.Specular.Administrations.Repositories.Contracts;
using NgSoftware.Specular.Common.Repositories;
using NgSoftware.Specular.Context.Contracts;

namespace NgSoftware.Specular.Administrations.Repositories;

/// <inheritdoc cref="ITeamWriteRepository"/>
public class TeamWriteRepository : BaseWriteRepository<Team>, ITeamWriteRepository, IAdministrationRepositoryAnchor
{
    /// <summary>
    /// Инициализирует новый экземпляр <see cref="TeamWriteRepository"/>
    /// </summary>
    public TeamWriteRepository(IDbWriterContext writerContext)
        : base(writerContext)
    {

    }
}
