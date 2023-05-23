using NgSoftware.Specular.Administrations.Entities;
using NgSoftware.Specular.Administrations.Repositories.Contracts;
using NgSoftware.Specular.Common.Repositories;
using NgSoftware.Specular.Context.Contracts;

namespace NgSoftware.Specular.Administrations.Repositories;

/// <inheritdoc cref="IUserWriteRepository"/>
public class UserWriteRepository : BaseWriteRepository<User>, IUserWriteRepository, IAdministrationRepositoryAnchor
{
    /// <summary>
    /// Инициализирует новый экземпляр <see cref="UserWriteRepository"/>
    /// </summary>
    public UserWriteRepository(IDbWriterContext writerContext)
        : base(writerContext)
    {

    }
}
