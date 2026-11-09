using System.Net;

using THH60Customer.Models;

namespace THH60Customer.Services
{
    public class CatalogService
    {
        private readonly HttpClient _httpClient;

        public CatalogService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<CatalogViewModel> GetCatalogAsync(
            string? search,
            int? categoryId)
        {
            var products = await GetProductsAsync();
            var categories = await GetCategoriesAsync();

            if (categoryId.HasValue)
            {
                products = products
                    .Where(product => product.ProdCatId == categoryId.Value)
                    .ToList();
            }

            if (!string.IsNullOrWhiteSpace(search))
            {
                var term = search.Trim();

                products = products
                    .Where(product =>
                        product.Description.Contains(
                            term,
                            StringComparison.OrdinalIgnoreCase) ||
                        (product.Manufacturer?.Contains(
                            term,
                            StringComparison.OrdinalIgnoreCase) ?? false))
                    .ToList();
            }

            var categoryNames = categories
                .ToDictionary(category => category.CategoryId, category => category.ProdCat);

            foreach (var product in products)
            {
                product.CategoryName = categoryNames.GetValueOrDefault(
                    product.ProdCatId,
                    "Uncategorized");
            }

            return new CatalogViewModel
            {
                Products = products
                    .OrderBy(product => product.Description)
                    .ToList(),
                Categories = categories,
                CategoryId = categoryId,
                Search = search
            };
        }

        public async Task<CustomerProduct?> GetProductAsync(int id)
        {
            var response = await _httpClient.GetAsync($"api/products/{id}");

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            await ApiResponse.EnsureSuccessAsync(response);

            var product = await response.Content.ReadFromJsonAsync<ProductDto>();

            return product == null ? null : Map(product);
        }

        private async Task<List<CustomerProduct>> GetProductsAsync()
        {
            var products = await _httpClient
                .GetFromJsonAsync<List<ProductDto>>("api/products")
                ?? [];

            return products.Select(Map).ToList();
        }

        private async Task<List<CustomerCategory>> GetCategoriesAsync()
        {
            var categories = await _httpClient
                .GetFromJsonAsync<List<CategoryDto>>("api/categories")
                ?? [];

            return categories
                .Select(category => new CustomerCategory
                {
                    CategoryId = category.CategoryId,
                    ProdCat = category.ProdCat
                })
                .ToList();
        }

        private static CustomerProduct Map(ProductDto product)
        {
            return new CustomerProduct
            {
                ProductId = product.ProductId,
                ProdCatId = product.ProdCatId,
                Description = product.Description ?? "",
                Manufacturer = product.Manufacturer,
                Stock = product.Stock,
                SellPrice = product.SellPrice
            };
        }

        private sealed class ProductDto
        {
            public int ProductId { get; set; }

            public int ProdCatId { get; set; }

            public string? Description { get; set; }

            public string? Manufacturer { get; set; }

            public int Stock { get; set; }

            public decimal? SellPrice { get; set; }
        }

        private sealed class CategoryDto
        {
            public int CategoryId { get; set; }

            public string ProdCat { get; set; } = "";
        }
    }
}
