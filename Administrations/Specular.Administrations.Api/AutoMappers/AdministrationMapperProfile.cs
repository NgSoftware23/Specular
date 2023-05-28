using AutoMapper;
using NgSoftware.Specular.Administrations.Api.Models.Organizations;
using NgSoftware.Specular.Administrations.Api.Models.Users;
using NgSoftware.Specular.Administrations.Services.Contracts.Models.Organizations;
using NgSoftware.Specular.Administrations.Services.Contracts.Models.User;

namespace NgSoftware.Specular.Administrations.Api.AutoMappers;

/// <inheritdoc />
public class AdministrationMapperProfile : Profile
{
    /// <summary>
    /// Инициализирует новый экземпляр <see cref="AdministrationMapperProfile"/>
    /// </summary>
    public AdministrationMapperProfile()
    {
        CreateMap<CreateUserApiModel, CreateUserModel>()
            .ValidateMemberList(MemberList.Destination);

        CreateMap<CreateOrganizationApiModel, CreateOrganizationModel>()
            .ValidateMemberList(MemberList.Destination);

        CreateMap<OrganizationModel, OrganizationApiModel>()
            .ValidateMemberList(MemberList.Destination);

        CreateMap<OrganizationApiModel, UpdateOrganizationModel>()
            .ForMember(x => x.UserId, opt => opt.Ignore())
            .ValidateMemberList(MemberList.Destination);
    }
}
