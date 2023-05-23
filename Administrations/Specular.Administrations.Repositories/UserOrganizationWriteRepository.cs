using NgSoftware.Specular.Administrations.Entities;
using NgSoftware.Specular.Administrations.Repositories.Contracts;
using NgSoftware.Specular.Common.Repositories;
using NgSoftware.Specular.Context.Contracts;

namespace NgSoftware.Specular.Administrations.Repositories;

/// <inheritdoc cref="IUserTeamWriteRepository"/>
public class UserOrganizationWriteRepository : BaseWriteRepository<UserOrganization>,
    IUserOrganizationWriteRepository,
    IAdministrationRepositoryAnchor
{
    /// <summary>
    /// Инициализирует новый экземпляр <see cref="UserOrganizationWriteRepository"/>
    /// </summary>
    public UserOrganizationWriteRepository(IDbWriterContext writerContext) : base(writerContext)
    {

    }
}
