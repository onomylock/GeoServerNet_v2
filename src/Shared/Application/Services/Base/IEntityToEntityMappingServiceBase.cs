using Microsoft.AspNetCore.Mvc.RazorPages;
using Shared.Domain.Entity.Base;

namespace Shared.Application.Services.Base;

public interface IEntityToEntityMappingServiceBase<TEntity> : IEntityServiceBase<TEntity> where TEntity : EntityBase
{
    Task<TEntity> GetByEntityLeftIdEntityRightIdAsync(
        Guid entityLeftId,
        Guid entityRightId,
        bool asNoTracking = false,
        CancellationToken cancellationToken = default
    );

    Task<(int total, IReadOnlyCollection<TEntity> entities)> GetByEntityLeftIdAsync(
        Guid entityLeftId,
        PageModel pageModel,
        bool asNoTracking = false,
        CancellationToken cancellationToken = default
    );

    Task<(int total, IReadOnlyCollection<TEntity> entities)> GetByEntityRightIdAsync(
        Guid entityRightId,
        PageModel pageModel,
        bool asNoTracking = false,
        CancellationToken cancellationToken = default
    );
}