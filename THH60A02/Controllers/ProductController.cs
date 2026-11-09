using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using TH_H60_A02.Models.Entities;
using TH_H60_A02.Models.ViewModels;
using TH_H60_A02.Services;

namespace TH_H60_A02.Controllers
{
    [Authorize(Roles = "manager,clerk")]
    public class ProductController : Controller
    {
        private readonly ProductService _productService;
        private readonly CategoryService _categoryService;

        public ProductController(
            ProductService productService,
            CategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index(string? search)
        {
            var products = await _productService.GetProducts();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var term = search.Trim();

                products = products
                    .Where(product =>
                        (product.Description?.Contains(
                            term,
                            StringComparison.OrdinalIgnoreCase) ?? false) ||
                        (product.Manufacturer?.Contains(
                            term,
                            StringComparison.OrdinalIgnoreCase) ?? false))
                    .ToList();
            }

            ViewData["Search"] = search;

            return View(products);
        }

        public async Task<IActionResult> UpdateStock(int id)
        {
            Product? product = await _productService.GetProduct(id);

            if (product == null)
            {
                return NotFound();
            }

            UpdateStockViewModel model = new UpdateStockViewModel
            {
                ProductId = product.ProductId,
                Description = product.Description,
                CurrentStock = product.Stock
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateStock(UpdateStockViewModel model)
        {
            try
            {
                await _productService.UpdateStock(
                    model.ProductId,
                    model.StockChange);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);

                return View(model);
            }
        }

        [Authorize(Roles = "manager")]
        public async Task<IActionResult> UpdatePrices(int id)
        {
            Product? product = await _productService.GetProduct(id);

            if (product == null)
            {
                return NotFound();
            }

            UpdatePricesViewModel model = new UpdatePricesViewModel
            {
                ProductId = product.ProductId,
                Description = product.Description,
                BuyPrice = product.BuyPrice?.ToString() ?? "",
                SellPrice = product.SellPrice?.ToString() ?? ""
            };

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "manager")]
        public async Task<IActionResult> UpdatePrices(UpdatePricesViewModel model)
        {
            try
            {
                if (!decimal.TryParse(model.BuyPrice, out decimal buyPrice))
                {
                    throw new ArithmeticException(
                        "Buy price must be numeric.");
                }

                if (!decimal.TryParse(model.SellPrice, out decimal sellPrice))
                {
                    throw new ArithmeticException(
                        "Sell price must be numeric.");
                }

                await _productService.UpdatePrices(
                    model.ProductId,
                    buyPrice,
                    sellPrice);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);

                return View(model);
            }
        }

        public async Task<IActionResult> Create()
        {
            ProductViewModel model = new()
            {
                Categories = await _categoryService.GetCategories(),
                CanManagePrices = User.IsInRole("manager")
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductViewModel model)
        {
            var canManagePrices = User.IsInRole("manager");

            model.CanManagePrices = canManagePrices;

            if (!canManagePrices)
            {
                model.BuyPrice = null;
                model.SellPrice = null;
            }

            if (!ModelState.IsValid)
            {
                model.Categories = await _categoryService.GetCategories();

                return View(model);
            }

            Product product = new()
            {
                ProdCatId = model.ProdCatId,
                Description = model.Description,
                Manufacturer = model.Manufacturer,
                Stock = model.Stock,
                BuyPrice = model.BuyPrice,
                SellPrice = model.SellPrice
            };

            try
            {
                await _productService.CreateProduct(product);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                model.Categories = await _categoryService.GetCategories();

                return View(model);
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            Product? product = await _productService.GetProduct(id);

            if (product == null)
            {
                return NotFound();
            }

            ProductViewModel model = new()
            {
                ProductId = product.ProductId,
                ProdCatId = product.ProdCatId,
                Description = product.Description,
                Manufacturer = product.Manufacturer,
                Stock = product.Stock,
                BuyPrice = product.BuyPrice,
                SellPrice = product.SellPrice,
                CanManagePrices = User.IsInRole("manager"),
                Categories = await _categoryService.GetCategories()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ProductViewModel model)
        {
            var canManagePrices = User.IsInRole("manager");

            if (!canManagePrices)
            {
                Product? existing = await _productService.GetProduct(model.ProductId);

                if (existing == null)
                {
                    return NotFound();
                }

                model.BuyPrice = existing.BuyPrice;
                model.SellPrice = existing.SellPrice;
            }

            Product product = new()
            {
                ProductId = model.ProductId,
                ProdCatId = model.ProdCatId,
                Description = model.Description,
                Manufacturer = model.Manufacturer,
                Stock = model.Stock,
                BuyPrice = model.BuyPrice,
                SellPrice = model.SellPrice
            };

            try
            {
                await _productService.UpdateProduct(product);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                model.CanManagePrices = canManagePrices;
                model.Categories = await _categoryService.GetCategories();

                return View(model);
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            Product? product = await _productService.GetProduct(id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Product product)
        {
            try
            {
                await _productService.DeleteProduct(product.ProductId);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);

                Product? existing = null;

                try
                {
                    existing = await _productService.GetProduct(product.ProductId);
                }
                catch
                {
                    // Fall back to the posted model if the service is unavailable.
                }

                return View(existing ?? product);
            }
        }

        public async Task<IActionResult> ProductsByCategory()
        {
            var products = await _productService.GetProductsByCategory();

            return View(products);
        }

        public async Task<IActionResult> ProductsForCategory()
        {
            ProductCategoryProductsViewModel model = new()
            {
                Categories = await _categoryService.GetCategories()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ProductsForCategory(ProductCategoryProductsViewModel model)
        {
            model.Categories = await _categoryService.GetCategories();

            model.Products =
                await _productService.GetProductsByCategoryId(
                    model.CategoryId);

            return View(model);
        }

        public async Task<IActionResult> Details(int id)
        {
            Product? product = await _productService.GetProduct(id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }
    }
}