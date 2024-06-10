using Core.Application.Common.Persistence;
using Core.Domain.Common.Models;

using Microsoft.EntityFrameworkCore;

namespace Core.Infrastructure.Persistence.Common.Repositories;

public abstract class SpecificationEvaluator<TEntity, TEntityId> : ISpecificationEvaluator<TEntity, TEntityId>
    where TEntity : Entity<TEntityId>
    where TEntityId : notnull
{
    public IQueryable<TEntity> GetQuery(
        IQueryable<TEntity> inputQuery, 
        ISpecification<TEntity, TEntityId> specification)
    {
        var query = inputQuery;

        query = ApplyCriteria(query, specification);
        query = ApplyIncludes(query, specification);
        query = ApplyOrdering(query, specification);
        query = ApplyGrouping(query, specification);
        query = ApplySelection(query, specification);
        query = ApplyCustomOrdering(query, specification);
        query = ApplyPaging(query, specification);

        return query;
    }

    private static IQueryable<TEntity> ApplyCriteria(
        IQueryable<TEntity> query, 
        ISpecification<TEntity, TEntityId> specification)
    {
        if (specification.Criteria is not null)
        {
            query = query.Where(specification.Criteria);
        }
        
        return query;
    }

    private static IQueryable<TEntity> ApplyIncludes(
        IQueryable<TEntity> query, 
        ISpecification<TEntity, TEntityId> specification)
    {
        query = specification.Includes
            .Aggregate(query, (current, include) => current.Include(include));
        
        query = specification.IncludeStrings.
            Aggregate(query, (current, include) => current.Include(include));
        
        return query;
    }

    private static IQueryable<TEntity> ApplyOrdering(
        IQueryable<TEntity> query, 
        ISpecification<TEntity, TEntityId> specification)
    {
        if (specification.OrderBy != null)
        {
            query = query.OrderBy(specification.OrderBy);
        }
        else if (specification.OrderByDescending != null)
        {
            query = query.OrderByDescending(specification.OrderByDescending);
        }
        
        return query;
    }

    private static IQueryable<TEntity> ApplyGrouping(IQueryable<TEntity> query, ISpecification<TEntity, TEntityId> specification)
    {
        if (specification.GroupBy != null)
        {
            query = query.GroupBy(specification.GroupBy).SelectMany(x => x);
        }
        
        return query;
    }

    private static IQueryable<TEntity> ApplySelection(
        IQueryable<TEntity> query, 
        ISpecification<TEntity, TEntityId> specification)
    {
        if (specification.Selector != null)
        {
            query = query.Select(specification.Selector);
        }
        
        return query;
    }

    private static IQueryable<TEntity> ApplyCustomOrdering(
        IQueryable<TEntity> query, 
        ISpecification<TEntity, TEntityId> specification)
    {
        if (specification.OrderByQueryable != null)
        {
            query = specification.OrderByQueryable(query);
        }
        
        return query;
    }

    private static IQueryable<TEntity> ApplyPaging(
        IQueryable<TEntity> query, 
        ISpecification<TEntity, TEntityId> specification)
    {
        if (specification.IsPagingEnabled)
        {
            query = query.Skip(specification.Skip).Take(specification.Take);
        }
        
        return query;
    }
}