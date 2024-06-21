using ProjectName.Domain.Common.Enums;

namespace ProjectName.Domain.Common.Persistence.Models;

public interface IResourceParameter
{
    int Page { get; set; }
    
    int PageSize { get; set; }

    OrderEnum? Order { get; set; }
}