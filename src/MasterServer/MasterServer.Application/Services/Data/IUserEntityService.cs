using MasterServer.Domain.Entities;
using Shared.Application.Services.Base;

namespace MasterServer.Application.Services.Data;

public interface IUserEntityService : IEntityServiceBase<User>
{
    Task<User> GetByEmailAsync(string email, bool asNoTracking = false, CancellationToken cancellationToken = default);
    Task<User> GetByAliasAsync(string alias, bool asNoTracking = false, CancellationToken cancellationToken = default);
}