using NgSoftware.Specular.Administrations.Entities;
using NgSoftware.Specular.Administrations.Repositories.Contracts;
using NgSoftware.Specular.Common.Repositories;
using NgSoftware.Specular.Context.Contracts;

namespace NgSoftware.Specular.Administrations.Repositories;

/// <inheritdoc cref="IOrganizationWriteRepository"/>
public class OrganizationWriteRepository : BaseWriteRepository<Organization>,
    IOrganizationWriteRepository,
    IAdministrationRepositoryAnchor
{
    /// <summary>
    /// Инициализирует новый экземпляр <see cref="OrganizationWriteRepository"/>
    /// </summary>
    public OrganizationWriteRepository(IDbWriterContext writerContext) : base(writerContext)
    {

    }
}
