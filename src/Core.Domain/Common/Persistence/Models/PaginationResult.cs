using Core.Domain.Common.Models;

namespace Core.Domain.Common.Persistence.Models;

public class PaginationResult<TEntity, TEntityId> 
    where TEntity : Entity<TEntityId>
    where TEntityId : notnull
{
    public PaginationResult(List<TEntity> items, int totalCount, IResourceParameter filter)
    {
        Items = items;
        Filter = filter;
        PaginationMetadata = new PaginationMetadata(totalCount, Filter.PageSize, Filter.Page);
    }
    
    protected PaginationResult()
    { 
    }
    
    public List<TEntity> Items { get; set; } = [];
   
    public PaginationMetadata PaginationMetadata { get; set; } = null!;
    
    private IResourceParameter Filter { get; set; } = null!;
}