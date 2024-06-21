using MapsterMapper;

using MediatR;

using Microsoft.AspNetCore.Mvc;

using ProjectName.Application.Common.Services;
using ProjectName.Presentation.Common.Controllers;
using ProjectName.Presentation.ForTestOnlyMustBeDeleted.Products.Dtos;

namespace ProjectName.Presentation.ForTestOnlyMustBeDeleted.Products.Controllers;

/// <summary>
/// This Controller is for testing purposes only.
/// </summary>
public class ProductsController(ISender sender, IMapper mapper, ICachingService cachingService) 
    : ApiController(sender, mapper)
{
    private static readonly Dictionary<Guid, ProductDto> Products = [];
    
    [HttpPost("{number:int}")]
    public async Task<ActionResult<ProductDto>> Create(int number, CancellationToken cancellationToken)
    {
        var product = new ProductDto { Id = Guid.NewGuid(), ProductName = $"Product No. {number}" };
        
        Products.Add(product.Id, product);

        await cachingService.RemoveAsync(key: "products", cancellationToken);
        
        return CreatedAtAction(
            actionName: nameof(Get),
            routeValues: new { id = product.Id },
            value: product);
    }
    
    [HttpPut]
    public async Task<ActionResult<ProductDto>> Update(ProductDto pr, CancellationToken cancellationToken)
    {
        var product = await GetProductFromDatabase(pr.Id);

        if (product is null)
        {
            return NotFound();
        }
        
        await cachingService.RemoveAsync(key: $"product-{pr.Id}", cancellationToken);

        Products.Remove(product.Id);
        
        Products.Add(product.Id, pr);
        
        return NoContent();
    }
    
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ProductDto>> Get(Guid id, CancellationToken cancellationToken)
    {
        var product = await cachingService.GetOrCreateAsync(
            key: $"product-{id}", 
            factory: () => GetProductFromDatabase(id), 
            cancellationToken);

        return product is null ? NotFound() : Ok(product);
    }
    
    [HttpGet]
    public async Task<ActionResult<List<ProductDto>>> Get(CancellationToken cancellationToken)
    {
        var products = await cachingService.GetOrCreateAsync(
            key: "products", 
            factory: async () => await GetProductsFromDatabase(), 
            cancellationToken);
        
        return Ok(products);
    }
    
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<List<ProductDto>>> Delete(Guid id, CancellationToken cancellationToken)
    {
        var product = await GetProductFromDatabase(id);

        if (product is null)
        {
            return NotFound();
        }
        
        await cachingService.RemoveAsync(key: $"product-{id}", cancellationToken);

        Products.Remove(product.Id);
        
        return NoContent();
    }

    private static async Task<ProductDto?> GetProductFromDatabase(Guid id)
    {
        await Task.Delay(3000);
        
        return Products.GetValueOrDefault(id);
    }
    
    private static async Task<List<ProductDto>> GetProductsFromDatabase()
    {
        await Task.Delay(3000);

        return Products.Values.ToList();
    }
}