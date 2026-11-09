using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using THH60Services.Interfaces;
using THH60Services.Models;
using THH60Services.Models.Entities;

namespace THH60Services.Controllers;

[ApiController]
[Route("api/products")]
public class ProductsController : ControllerBase
{
    private readonly IProductRepository _productRepository;
    private readonly IProductCategoryRepository _categoryRepository;

    public ProductsController(
        IProductRepository productRepository,
        IProductCategoryRepository categoryRepository)
    {
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
    }

    [HttpGet]
    public IActionResult GetProducts()
    {
        return Ok(_productRepository.GetProducts());
    }

    [HttpGet("{id}")]
    public IActionResult GetProduct(int id)
    {
        var product = _productRepository.GetDetails(id);

        if (product == null)
        {
            return NotFound();
        }

        return Ok(product);
    }
    [HttpGet("bycategory")]
    public IActionResult GetProductsByCategory()
    {
        return Ok(_productRepository.GetProductsByCategory());
    }

    [HttpGet("category/{categoryId}")]
    public IActionResult GetProductsByCategoryId(int categoryId)
    {
        return Ok(_productRepository.GetProductsByCategoryId(categoryId));
    }

    [HttpPost]
    public IActionResult CreateProduct(Product product)
    {
        ValidateProduct(product);

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            _productRepository.Add(product);
        }
        catch (DbUpdateException)
        {
            return BadRequest(new
            {
                message = "The product could not be saved. Check the category and product values."
            });
        }

        return CreatedAtAction(
            nameof(GetProduct),
            new { id = product.ProductId },
            product);
    }

    [HttpPut("{id}")]
    public IActionResult UpdateProduct(int id, Product product)
    {
        if (id != product.ProductId)
        {
            return BadRequest();
        }

        if (_productRepository.GetById(id) == null)
        {
            return NotFound();
        }

        ValidateProduct(product);

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            _productRepository.Update(product);
        }
        catch (DbUpdateException)
        {
            return BadRequest(new
            {
                message = "The product could not be updated. Check the category and product values."
            });
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteProduct(int id)
    {
        if (_productRepository.GetById(id) == null)
        {
            return NotFound();
        }

        try
        {
            _productRepository.Delete(id);
        }
        catch (DbUpdateException)
        {
            return BadRequest(new
            {
                message = "Cannot delete a product that belongs to a shopping cart or order."
            });
        }

        return NoContent();
    }

    [HttpPut("{id}/stock")]
    public IActionResult UpdateStock(int id, UpdateStockRequest request)
    {
        if (_productRepository.GetById(id) == null)
        {
            return NotFound();
        }

        try
        {
            _productRepository.UpdateStock(id, request.StockChange);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }

        return NoContent();
    }

    [HttpPut("{id}/prices")]
    public IActionResult UpdatePrices(int id, [FromBody] UpdatePricesRequest request)
    {
        if (_productRepository.GetById(id) == null)
        {
            return NotFound();
        }

        try
        {
            _productRepository.UpdatePrices(id, request.BuyPrice, request.SellPrice);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }

        return NoContent();
    }

    private void ValidateProduct(Product product)
    {
        if (string.IsNullOrWhiteSpace(product.Description))
        {
            ModelState.AddModelError(
                nameof(product.Description),
                "Description is required.");
        }
        else if (product.Description.Length > 200)
        {
            ModelState.AddModelError(
                nameof(product.Description),
                "Description cannot exceed 200 characters.");
        }

        if (product.Manufacturer?.Length > 100)
        {
            ModelState.AddModelError(
                nameof(product.Manufacturer),
                "Manufacturer cannot exceed 100 characters.");
        }

        if (product.Stock < 0)
        {
            ModelState.AddModelError(
                nameof(product.Stock),
                "Stock cannot be negative.");
        }

        if (product.ProdCatId <= 0 ||
            _categoryRepository.GetById(product.ProdCatId) == null)
        {
            ModelState.AddModelError(
                nameof(product.ProdCatId),
                "Select a valid category.");
        }

        if (product.BuyPrice < 0)
        {
            ModelState.AddModelError(
                nameof(product.BuyPrice),
                "Buy price cannot be negative.");
        }

        if (product.SellPrice < 0)
        {
            ModelState.AddModelError(
                nameof(product.SellPrice),
                "Sell price cannot be negative.");
        }

        if (product.BuyPrice.HasValue &&
            product.SellPrice.HasValue &&
            product.SellPrice < product.BuyPrice)
        {
            ModelState.AddModelError(
                nameof(product.SellPrice),
                "Sell price cannot be less than buy price.");
        }
    }
}