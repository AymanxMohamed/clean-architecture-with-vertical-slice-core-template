using ProjectName.Domain.Common.Models;

namespace ProjectName.Application.Common.Persistence;

public interface ISpecificationEvaluator<TEntity, TEntityId>
    where TEntity : Entity<TEntityId>
    where TEntityId : notnull
{
    IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery, ISpecification<TEntity, TEntityId> specification);
}