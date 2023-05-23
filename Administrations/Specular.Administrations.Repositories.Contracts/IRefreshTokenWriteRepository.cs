﻿using NgSoftware.Specular.Administrations.Entities;
using NgSoftware.Specular.Common.Repositories.Contracts;

namespace NgSoftware.Specular.Administrations.Repositories.Contracts;

/// <summary>
/// Репозиторий записи <see cref="RefreshToken"/>
/// </summary>
public interface IRefreshTokenWriteRepository : IDbWriter<RefreshToken>
{

}
