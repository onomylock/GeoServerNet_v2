using MasterServer.Application.Repository;
using MasterServer.Infrastructure.Data;
using Microsoft.Extensions.Logging;
using Shared.Domain.Entity.Base;
using Shared.Infrastructure.Data;
using Shared.Infrastructure.Repository;

namespace MasterServer.Infrastructure.Repository;

public class MasterServerRepository<TEntity>(MasterServerDbContext dbContext, ILogger<DbContextAction<MasterServerDbContext>> logger)
    : RepositoryBase<TEntity, MasterServerDbContext>(dbContext, logger), IMasterServerRepository<TEntity>
    where TEntity : EntityBase;