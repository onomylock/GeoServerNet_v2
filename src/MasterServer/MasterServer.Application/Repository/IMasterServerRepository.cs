using Shared.Application.Repository;
using Shared.Domain.Entity.Base;

namespace MasterServer.Application.Repository;

public interface IMasterServerRepository<TEntity> : IRepositoryBase<TEntity> where TEntity : EntityBase;