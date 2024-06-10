using System.Linq.Expressions;

using Core.Domain.Common.Models;
using Core.Domain.Common.Persistence.Models;

namespace Core.Application.Common.Persistence;

public abstract class SpecificationBase<TEntity, TEntityId> : ISpecification<TEntity, TEntityId>
    where TEntity : Entity<TEntityId>
    where TEntityId : notnull
{
    protected SpecificationBase(Expression<Func<TEntity, bool>> criteria)
    {
        Criteria = criteria;
    }
    
    protected SpecificationBase()
    {
    }
    
    public Expression<Func<TEntity, bool>> Criteria { get; } = null!;
    
    public List<Expression<Func<TEntity, object>>> Includes { get; } = [];
    
    public List<string> IncludeStrings { get; } = [];
    
    public Expression<Func<TEntity, object>> OrderBy { get; private set; } = null!;
    
    public Expression<Func<TEntity, object>> OrderByDescending { get; private set; } = null!;
    
    public Expression<Func<TEntity, object>> GroupBy { get; private set; } = null!;
    
    public Expression<Func<TEntity, TEntity>> Selector { get; private set; } = null!;
    
    public int Take { get; private set; }
    
    public int Skip { get; private set; }
    
    public bool IsPagingEnabled { get; set; }
    
    public IResourceParameter ResourceParameter { get; protected set; } = null!;
    
    public Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> OrderByQueryable { get; private set; }
    
    protected virtual void AddInclude(Expression<Func<TEntity, object>> includeExpression)
    {
        Includes.Add(includeExpression);
    }

    protected virtual void AddInclude(string includeString)
    {
        IncludeStrings.Add(includeString);
    }
    
    protected virtual void AddSelector(Expression<Func<TEntity, TEntity>> select)
    {
        Selector = select;
    }
    
    protected virtual void ApplyPaging(int page, int pageSize)
    {
        Skip = (page - 1) * pageSize;
        Take = pageSize;
        IsPagingEnabled = true;
    }

    protected virtual void ApplyOrderBy(Expression<Func<TEntity, object>> orderByExpression)
    {
        OrderBy = orderByExpression;
    }
    
    protected virtual void ApplyOrderByQueryable(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> order)
    {
        OrderByQueryable = order;
    }
    
    protected virtual void ApplyOrderByDescending(Expression<Func<TEntity, object>> orderByDescendingExpression)
    {
        OrderByDescending = orderByDescendingExpression;
    }

    protected virtual void ApplyGroupBy(Expression<Func<TEntity, object>> groupByExpression)
    {
        GroupBy = groupByExpression;
    }
}