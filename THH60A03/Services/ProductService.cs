using TH_H60_A03.Models.Entities;

namespace TH_H60_A03.Services
{
    public class ProductService
    {
        private readonly HttpClient _httpClient;

        public ProductService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Product>> GetProducts()
        {
            return await _httpClient.GetFromJsonAsync<List<Product>>(
                "api/products")
                ?? [];
        }

        public async Task<Product?> GetProduct(int id)
        {
            return await _httpClient.GetFromJsonAsync<Product>(
                $"api/products/{id}");
        }

        public async Task<List<Product>> GetProductsByCategory()
        {
            return await _httpClient.GetFromJsonAsync<List<Product>>(
                "api/products/bycategory")
                ?? [];
        }

        public async Task<List<Product>> GetProductsByCategoryId(int categoryId)
        {
            return await _httpClient.GetFromJsonAsync<List<Product>>(
                $"api/products/category/{categoryId}")
                ?? [];
        }

        public async Task CreateProduct(Product product)
        {
            var response = await _httpClient.PostAsJsonAsync(
                "api/products",
                product);

            await ApiResponse.EnsureSuccessAsync(response);
        }

        public async Task UpdateProduct(Product product)
        {
            var response = await _httpClient.PutAsJsonAsync(
                $"api/products/{product.ProductId}",
                product);

            await ApiResponse.EnsureSuccessAsync(response);
        }

        public async Task DeleteProduct(int id)
        {
            var response = await _httpClient.DeleteAsync(
                $"api/products/{id}");

            await ApiResponse.EnsureSuccessAsync(response);
        }

        public async Task UpdateStock(int productId, int stockChange)
        {
            var response = await _httpClient.PutAsJsonAsync(
                $"api/products/{productId}/stock",
                new
                {
                    StockChange = stockChange
                });

            await ApiResponse.EnsureSuccessAsync(response);
        }

        public async Task UpdatePrices(int productId, decimal buyPrice, decimal sellPrice)
        {
            var response = await _httpClient.PutAsJsonAsync(
                $"api/products/{productId}/prices",
                new { BuyPrice = buyPrice, SellPrice = sellPrice });

            await ApiResponse.EnsureSuccessAsync(response);
        }
    }
}
