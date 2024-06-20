namespace Core.Presentation.Products.Dtos;

public class ProductDto
{
    public Guid Id { get; init; }
    
    public string ProductName { get; set; } = null!;
}