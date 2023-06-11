using NgSoftware.Specular.Administrations.Services.Contracts.Models.Teams;

namespace NgSoftware.Specular.Administrations.Services.Contracts.Interfaces;

/// <summary>
/// Управление командами
/// </summary>
public interface ITeamManager
{
    /// <summary>
    /// Получает все команды пользователя
    /// </summary>
    Task<IReadOnlyCollection<TeamModel>> GetTeamsByUserIdAsync(Guid userId, CancellationToken cancellationToken);

    /// <summary>
    /// Создает команду
    /// </summary>
    Task<TeamModel> CreateAsync(CreateTeamModel model, CancellationToken cancellationToken);

    /// <summary>
    /// Обновляет команду
    /// </summary>
    Task<TeamModel> UpdateAsync(UpdateTeamModel model, CancellationToken cancellationToken);

    /// <summary>
    /// Удаляет команду
    /// </summary>
    Task DeleteAsync(DeleteTeamModel model, CancellationToken cancellationToken);
}
