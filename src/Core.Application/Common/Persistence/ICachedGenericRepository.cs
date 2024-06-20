using Core.Domain.Common.Models;

namespace Core.Application.Common.Persistence;

public interface ICachedGenericRepository<TEntity, TEntityId> : IGenericRepository<TEntity, TEntityId>
    where TEntity : Entity<TEntityId>
    where TEntityId : notnull;