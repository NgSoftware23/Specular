using NgSoftware.Specular.Administrations.Entities;
using NgSoftware.Specular.Administrations.Repositories.Contracts;
using NgSoftware.Specular.Common.Repositories;
using NgSoftware.Specular.Context.Contracts;

namespace NgSoftware.Specular.Administrations.Repositories;

/// <inheritdoc cref="IRefreshTokenWriteRepository"/>
public class RefreshTokenWriteRepository : BaseWriteRepository<RefreshToken>,
    IRefreshTokenWriteRepository,
    IAdministrationRepositoryAnchor
{
    /// <summary>
    /// Инициализирует новый экземпляр <see cref="RefreshTokenWriteRepository"/>
    /// </summary>
    public RefreshTokenWriteRepository(IDbWriterContext writerContext) : base(writerContext)
    {

    }
}
