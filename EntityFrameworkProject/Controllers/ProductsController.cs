using Microsoft.AspNetCore.Mvc;
using EntityFrameworkProject.Repositories;
using SharedModels.Models;

namespace EntityFrameworkProject.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly ProductRepository _repository;

    public ProductsController(ProductRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
    {
        var products = await _repository.GetAllAsync();
        return Ok(products);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        var product = await _repository.GetByIdAsync(id);
        if (product == null)
            return NotFound();

        return Ok(product);
    }

    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct(Product product)
    {
        var createdProduct = await _repository.CreateAsync(product);
        return CreatedAtAction(nameof(GetProduct), new { id = createdProduct.Id }, createdProduct);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct(int id, Product product)
    {
        var updatedProduct = await _repository.UpdateAsync(id, product);
        if (updatedProduct == null)
            return NotFound();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var result = await _repository.DeleteAsync(id);
        if (!result)
            return NotFound();

        return NoContent();
    }

    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<Product>>> SearchProducts([FromQuery] string name)
    {
        var products = await _repository.SearchByNameAsync(name);
        return Ok(products);
    }

    [HttpGet("price-range")]
    public async Task<ActionResult<IEnumerable<Product>>> GetProductsByPriceRange(
        [FromQuery] decimal minPrice, 
        [FromQuery] decimal maxPrice)
    {
        var products = await _repository.GetByPriceRangeAsync(minPrice, maxPrice);
        return Ok(products);
    }
} 