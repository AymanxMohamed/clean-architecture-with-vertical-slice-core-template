using ProjectName.Domain.Common.Models;

namespace ProjectName.Application.Common.Persistence;

public interface ICachedGenericRepository<TEntity, TEntityId> : IGenericRepository<TEntity, TEntityId>
    where TEntity : Entity<TEntityId>
    where TEntityId : notnull;