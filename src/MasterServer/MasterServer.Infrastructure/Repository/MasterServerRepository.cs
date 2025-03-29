using MasterServer.Application.Repository;
using MasterServer.Infrastructure.Data;
using Shared.Domain.Entity.Base;
using Shared.Infrastructure.Repository;

namespace MasterServer.Infrastructure.Repository;

public class MasterServerRepository<TEntity>(
    MasterServerDbContext dbContext)
    : RepositoryBase<TEntity, MasterServerDbContext>(dbContext), IMasterServerRepository<TEntity>
    where TEntity : EntityBase;