using Shared.Application.Repository;
using Shared.Domain.Entity.Base;

namespace NodeServer.Application.Repository;

public interface INodeServerRepository<TEntity> : IRepositoryBase<TEntity> where TEntity : EntityBase;