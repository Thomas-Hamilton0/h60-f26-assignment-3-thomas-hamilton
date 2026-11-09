using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using THH60Services.Interfaces;
using THH60Services.Models.Entities;

namespace THH60Services.Controllers;

[ApiController]
[Route("api/categories")]
public class CategoriesController : ControllerBase
{
    private readonly IProductCategoryRepository _productCategoryRepository;

    public CategoriesController(IProductCategoryRepository productCategoryRepository)
    {
        _productCategoryRepository = productCategoryRepository;
    }

    [HttpGet]
    public IActionResult GetCategories()
    {
        return Ok(_productCategoryRepository.GetCategories());
    }

    [HttpGet("{id}")]
    public IActionResult GetCategory(int id)
    {
        var category = _productCategoryRepository.GetById(id);

        if (category == null)
        {
            return NotFound();
        }

        return Ok(category);
    }

    [HttpPost]
    public IActionResult CreateCategory(ProductCategory category)
    {
        ValidateCategory(category);

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            _productCategoryRepository.Add(category);
        }
        catch (DbUpdateException)
        {
            return BadRequest(new
            {
                message = "The category could not be saved."
            });
        }

        return CreatedAtAction(
            nameof(GetCategory),
            new { id = category.CategoryId },
            category);
    }

    [HttpPut("{id}")]
    public IActionResult UpdateCategory(
        int id,
        ProductCategory category)
    {
        if (id != category.CategoryId)
        {
            return BadRequest();
        }

        if (_productCategoryRepository.GetById(id) == null)
        {
            return NotFound();
        }

        ValidateCategory(category, id);

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            _productCategoryRepository.Update(category);
        }
        catch (DbUpdateException)
        {
            return BadRequest(new
            {
                message = "The category could not be updated."
            });
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteCategory(int id)
    {
        if (_productCategoryRepository.GetById(id) == null)
        {
            return NotFound();
        }

        try
        {
            _productCategoryRepository.Delete(id);
        }
        catch (DbUpdateException)
        {
            return BadRequest(new
            {
                message = "Cannot delete a category containing products."
            });
        }

        return NoContent();
    }

    private void ValidateCategory(
        ProductCategory category,
        int? currentCategoryId = null)
    {
        if (string.IsNullOrWhiteSpace(category.ProdCat))
        {
            ModelState.AddModelError(
                nameof(category.ProdCat),
                "Category name is required.");

            return;
        }

        if (category.ProdCat.Length > 60)
        {
            ModelState.AddModelError(
                nameof(category.ProdCat),
                "Category name cannot exceed 60 characters.");
        }

        var duplicate = _productCategoryRepository
            .GetCategories()
            .Any(existing =>
                existing.CategoryId != (currentCategoryId ?? 0) &&
                string.Equals(
                    existing.ProdCat,
                    category.ProdCat,
                    StringComparison.OrdinalIgnoreCase));

        if (duplicate)
        {
            ModelState.AddModelError(
                nameof(category.ProdCat),
                "A category with this name already exists.");
        }
    }
}