using System.Linq.Expressions;

using Core.Domain.Common.Models;
using Core.Domain.Common.Persistence.Models;

namespace Core.Application.Common.Persistence;

public interface ISpecification<TEntity, TEntityId>
    where TEntity : Entity<TEntityId>
    where TEntityId : notnull
{
    Expression<Func<TEntity, bool>>? Criteria { get; }
    
    List<Expression<Func<TEntity, object>>> Includes { get; }
    
    List<string> IncludeStrings { get; }
    
    Expression<Func<TEntity, object>>? OrderBy { get; }
    
    Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? OrderByQueryable { get; }
    
    Expression<Func<TEntity, object>>? OrderByDescending { get; }
    
    Expression<Func<TEntity, object>>? GroupBy { get; }
    
    Expression<Func<TEntity, TEntity>>? Selector { get; }
    
    IResourceParameter ResourceParameter { get; }
    
    int Take { get; }
    
    int Skip { get; }
    
    bool IsPagingEnabled { get; set; }
}