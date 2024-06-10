using System.Linq.Expressions;

using Core.Domain.Common.Persistence.Models;

namespace Core.Application.Common.Persistence;

public interface ISpecification<T>
{
    Expression<Func<T, bool>> Criteria { get; }
    List<Expression<Func<T, object>>> Includes { get; }
    List<string> IncludeStrings { get; }
    Expression<Func<T, object>> OrderBy { get; }
    Func<IQueryable<T>, IOrderedQueryable<T>> OrderByQueryable { get; }
    Expression<Func<T, object>> OrderByDescending { get; }
    Expression<Func<T, object>> GroupBy { get; }
    Expression<Func<T, T>> Selector { get; }
    IResourceParameter ResourceParameter { get; }
    int Take { get; }
    int Skip { get; }
    bool IsPagingEnabled { get; set; }
}