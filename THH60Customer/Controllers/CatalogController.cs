using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using THH60Customer.Models;
using THH60Customer.Services;

namespace THH60Customer.Controllers
{
    [Authorize(Roles = "customer")]
    public class CatalogController : Controller
    {
        private readonly CatalogService _catalogService;

        public CatalogController(CatalogService catalogService)
        {
            _catalogService = catalogService;
        }

        public async Task<IActionResult> Index(string? search, int? categoryId)
        {
            try
            {
                var model = await _catalogService.GetCatalogAsync(search, categoryId);

                return View(model);
            }
            catch (Exception ex)
            {
                ViewData["Error"] = ex.Message;

                return View(new CatalogViewModel
                {
                    Search = search,
                    CategoryId = categoryId
                });
            }
        }

        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var product = await _catalogService.GetProductAsync(id);

                if (product == null)
                {
                    return NotFound();
                }

                return View(product);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;

                return RedirectToAction(nameof(Index));
            }
        }
    }
}
