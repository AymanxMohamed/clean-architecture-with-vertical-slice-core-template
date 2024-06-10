using Core.Domain.Common.Enums;

namespace Core.Domain.Common.Persistence.Models;

public interface IResourceParameter
{
    int Page { get; set; }
    
    int PageSize { get; set; }

    OrderEnum? Order { get; set; }
}