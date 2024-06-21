namespace Core.Presentation.ForTestOnlyMustBeDeleted.Products.Dtos;

public class ProductDto
{
    public Guid Id { get; init; }
    
    public string ProductName { get; set; } = null!;
}