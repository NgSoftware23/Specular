using AutoMapper;
using NgSoftware.Specular.Administrations.Entities.Enums;
using NgSoftware.Specular.Administrations.Repositories.Contracts;
using NgSoftware.Specular.Administrations.Services.Contracts.Exceptions;
using NgSoftware.Specular.Administrations.Services.Contracts.Interfaces;
using NgSoftware.Specular.Administrations.Services.Contracts.Models.Organizations;
using NgSoftware.Specular.Context.Contracts;

namespace NgSoftware.Specular.Administrations.Services.Organization;

/// <inheritdoc cref="IOrganizationManager"/>
public class OrganizationManager : IOrganizationManager, IAdministrationsServiceAnchor
{
    private readonly IOrganizationReadRepository organizationReadRepository;
    private readonly IOrganizationWriteRepository organizationWriteRepository;
    private readonly IUserOrganizationReadRepository userOrganizationReadRepository;
    private readonly IUserOrganizationWriteRepository userOrganizationWriteRepository;
    private readonly ITeamReadRepository teamReadRepository;
    private readonly ITeamWriteRepository teamWriteRepository;
    private readonly IUserTeamReadRepository userTeamReadRepository;
    private readonly IUserTeamWriteRepository userTeamWriteRepository;
    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper mapper;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="OrganizationManager"/>
    /// </summary>
    public OrganizationManager(IOrganizationReadRepository organizationReadRepository,
        IOrganizationWriteRepository organizationWriteRepository,
        IUserOrganizationReadRepository userOrganizationReadRepository,
        IUserOrganizationWriteRepository userOrganizationWriteRepository,
        ITeamReadRepository teamReadRepository,
        ITeamWriteRepository teamWriteRepository,
        IUserTeamReadRepository userTeamReadRepository,
        IUserTeamWriteRepository userTeamWriteRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        this.organizationReadRepository = organizationReadRepository;
        this.organizationWriteRepository = organizationWriteRepository;
        this.userOrganizationReadRepository = userOrganizationReadRepository;
        this.userOrganizationWriteRepository = userOrganizationWriteRepository;
        this.teamReadRepository = teamReadRepository;
        this.teamWriteRepository = teamWriteRepository;
        this.userTeamReadRepository = userTeamReadRepository;
        this.userTeamWriteRepository = userTeamWriteRepository;
        this.unitOfWork = unitOfWork;
        this.mapper = mapper;
    }

    async Task<OrganizationModel> IOrganizationManager.CreateAsync(CreateOrganizationModel model, CancellationToken cancellationToken)
    {
        var organization = new Entities.Organization
        {
            Id = Guid.NewGuid(),
            Name = model.Name,
            NameLowerCase = model.Name.ToLower(),
            Description = model.Description,
        };
        organizationWriteRepository.Add(organization);
        var userOrganization = new Entities.UserOrganization
        {
            Id = Guid.NewGuid(),
            OrganizationId = organization.Id,
            UserId = model.UserId,
            Role = Role.Admin,
        };
        userOrganizationWriteRepository.Add(userOrganization);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return mapper.Map<OrganizationModel>(organization);
    }

    async Task<IEnumerable<OrganizationModel>> IOrganizationManager.GetByUserIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        var items = await organizationReadRepository.GetByUserIdAsync(userId, cancellationToken);
        return mapper.Map<IEnumerable<OrganizationModel>>(items);
    }

    async Task<OrganizationModel> IOrganizationManager.UpdateAsync(UpdateOrganizationModel model, CancellationToken cancellationToken)
    {
        var organization = await GetOrganization(model.Id, model.UserId, cancellationToken);
        organization.Name = model.Name;
        organization.NameLowerCase = model.Name.ToLower();
        organization.Description = model.Description;

        organizationWriteRepository.Update(organization);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return mapper.Map<OrganizationModel>(organization);
    }

    async Task IOrganizationManager.DeleteAsync(DeleteOrganizationModel model, CancellationToken cancellationToken)
    {
        var organization = await GetOrganization(model.OrganizationId, model.UserId, cancellationToken);

        organizationWriteRepository.Delete(organization);

        var userOrganizations = await userOrganizationReadRepository.GetByOrganizationIdAsync(model.OrganizationId, cancellationToken);
        foreach (var item in userOrganizations)
        {
            userOrganizationWriteRepository.Delete(item);
        }

        var teams = await teamReadRepository.GetByOrganizationIdAsync(model.OrganizationId, cancellationToken);
        foreach (var item in teams)
        {
            teamWriteRepository.Delete(item);
        }

        var userTeams = await userTeamReadRepository.GetByTeamIdsAsync(teams.Select(x => x.Id).ToArray(), cancellationToken);
        foreach (var item in userTeams)
        {
            userTeamWriteRepository.Delete(item);
        }
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }

    private async Task<Entities.Organization> GetOrganization(Guid organizationId, Guid userId, CancellationToken cancellationToken)
    {
        var organization = await organizationReadRepository.GetActiveByIdAsync(organizationId, cancellationToken);
        if (organization == null)
        {
            throw new AdministrationEntityNotFoundException<Entities.Organization>(organizationId);
        }

        var userOrganization = await userOrganizationReadRepository.GetByUserAndOrganizationIdAsync(userId, organizationId, cancellationToken);
        if (userOrganization?.Role != Role.Admin)
        {
            throw new AdministrationInvalidOperationException("Недостаточно прав для удаления организации");
        }

        return organization;
    }
}
