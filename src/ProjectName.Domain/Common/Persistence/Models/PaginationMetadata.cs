namespace ProjectName.Domain.Common.Persistence.Models;

public class PaginationMetadata
{
    public PaginationMetadata(int totalItemCount, int pageSize, int currentPage)
    {
        TotalItemCount = totalItemCount;
        PageSize = pageSize;
        CurrentPage = currentPage;
        TotalPageCount = (int)Math.Ceiling(totalItemCount / (double)PageSize);
        HasNext = CurrentPage < TotalPageCount;
        HasPrevious = CurrentPage > 1;
    }
    
    public int TotalItemCount { get; set; }
    
    public int TotalPageCount { get; private set; }
    
    public int PageSize { get; set; }
    
    public int CurrentPage { get; set; }
    
    public bool HasNext { get; private set; }
    
    public bool HasPrevious { get; private set; }
}