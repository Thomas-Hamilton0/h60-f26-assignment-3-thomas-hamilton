using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using TH_H60_A03.Models.Entities;
using TH_H60_A03.Services;

namespace TH_H60_A03.Controllers
{
    [Authorize(Roles = "manager,clerk")]
    public class ProductCategoryController : Controller
    {
        private readonly CategoryService _categoryService;

        public ProductCategoryController(
            CategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index(string? search)
        {
            var categories = await _categoryService.GetCategories();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var term = search.Trim();

                categories = categories
                    .Where(category => category.ProdCat.Contains(
                        term,
                        StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            ViewData["Search"] = search;

            return View(categories);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductCategory category)
        {
            try
            {
                await _categoryService.CreateCategory(category);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);

                return View(category);
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            ProductCategory? category =
                await _categoryService.GetCategory(id);

            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ProductCategory category)
        {
            try
            {
                await _categoryService.UpdateCategory(category);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);

                return View(category);
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            ProductCategory? category =
                await _categoryService.GetCategory(id);

            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(ProductCategory category)
        {
            try
            {
                await _categoryService.DeleteCategory(category.CategoryId);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);

                return View(category);
            }
        }
    }
}