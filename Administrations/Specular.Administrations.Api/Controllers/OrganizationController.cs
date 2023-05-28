using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NgSoftware.Specular.Administrations.Api.Models.Organizations;
using NgSoftware.Specular.Administrations.Api.Resources;
using NgSoftware.Specular.Administrations.Services.Contracts.Interfaces;
using NgSoftware.Specular.Administrations.Services.Contracts.Models.Organizations;
using NgSoftware.Specular.Common.Core.Contracts;
using NgSoftware.Specular.Common.Mvc.Attributes;

namespace NgSoftware.Specular.Administrations.Api.Controllers;

/// <summary>
/// Работа с организациями
/// </summary>
[ApiController]
[Authorize]
[ApiExplorerSettings(GroupName = $"{AdministrationConstants.DocPrefix}v1")]
[Route(AdministrationConstants.DefaultControllerRoute)]
public class OrganizationController : ControllerBase
{
    private readonly IOrganizationManager organizationManager;
    private readonly IAdministrationValidateService administrationValidateService;
    private readonly IIdentityProvider identityProvider;
    private readonly IMapper mapper;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="OrganizationController"/>
    /// </summary>
    public OrganizationController(IOrganizationManager organizationManager,
        IAdministrationValidateService administrationValidateService,
        IIdentityProvider identityProvider,
        IMapper mapper)
    {
        this.organizationManager = organizationManager;
        this.administrationValidateService = administrationValidateService;
        this.identityProvider = identityProvider;
        this.mapper = mapper;
    }

    /// <summary>
    /// Получает список организаций
    /// </summary>
    [HttpGet]
    [ApiOk(typeof(IEnumerable<OrganizationApiModel>))]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        var result = await organizationManager.GetByUserIdAsync(identityProvider.Id, cancellationToken);
        return Ok(mapper.Map<IEnumerable<OrganizationApiModel>>(result));
    }

    /// <summary>
    /// Создаёт новую организацию
    /// </summary>
    [HttpPost]
    [ApiOk(typeof(OrganizationApiModel))]
    [ApiConflict]
    public async Task<IActionResult> Create(CreateOrganizationApiModel request, CancellationToken cancellationToken)
    {
        var model = mapper.Map<CreateOrganizationModel>(request);
        await administrationValidateService.ValidateAsync(model, cancellationToken);
        var result = await organizationManager.CreateAsync(model, cancellationToken);
        return Ok(mapper.Map<OrganizationApiModel>(result));
    }

    /// <summary>
    /// Обновляет организацию
    /// </summary>
    [HttpPut]
    [ApiOk(typeof(OrganizationApiModel))]
    [ApiNotAcceptable]
    [ApiNotFound]
    [ApiConflict]
    public async Task<IActionResult> Update(OrganizationApiModel request, CancellationToken cancellationToken)
    {
        var model = mapper.Map<UpdateOrganizationModel>(request);
        model.UserId = identityProvider.Id;
        await administrationValidateService.ValidateAsync(model, cancellationToken);
        var result = await organizationManager.UpdateAsync(model, cancellationToken);
        return Ok(mapper.Map<OrganizationApiModel>(result));
    }

    /// <summary>
    /// Удаляет организацию
    /// </summary>
    [HttpDelete("{id}")]
    [ApiNoContent]
    [ApiNotAcceptable]
    [ApiNotFound]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var model = new DeleteOrganizationModel
        {
            OrganizationId = id,
            UserId = identityProvider.Id
        };
        await organizationManager.DeleteAsync(model, cancellationToken);
        return NoContent();
    }
}
