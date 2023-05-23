using AutoMapper;
using NgSoftware.Specular.Administrations.Services.Contracts.Models.Organizations;
using NgSoftware.Specular.Administrations.Services.Contracts.Models.User;

namespace NgSoftware.Specular.Administrations.Services.AutoMappers;

/// <inheritdoc />
public class AdministrationServiceProfile : Profile
{
    /// <summary>
    /// Инициализирует новый экземпляр <see cref="AdministrationServiceProfile"/>
    /// </summary>
    public AdministrationServiceProfile()
    {
        CreateMap<Entities.User, UserLoggedModel>()
            .ValidateMemberList(MemberList.Destination);

        CreateMap<Entities.Organization, OrganizationModel>()
            .ValidateMemberList(MemberList.Destination);
    }
}
