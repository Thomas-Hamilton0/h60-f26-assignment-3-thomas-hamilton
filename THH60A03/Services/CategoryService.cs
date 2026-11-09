using TH_H60_A03.Models.Entities;

namespace TH_H60_A03.Services
{
    public class CategoryService
    {
        private readonly HttpClient _httpClient;

        public CategoryService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<ProductCategory>> GetCategories()
        {
            return await _httpClient.GetFromJsonAsync<List<ProductCategory>>(
                "api/categories")
                ?? [];
        }

        public async Task<ProductCategory?> GetCategory(int id)
        {
            return await _httpClient.GetFromJsonAsync<ProductCategory>(
                $"api/categories/{id}");
        }

        public async Task CreateCategory(ProductCategory category)
        {
            var response = await _httpClient.PostAsJsonAsync(
                "api/categories",
                category);

            await ApiResponse.EnsureSuccessAsync(response);
        }

        public async Task UpdateCategory(ProductCategory category)
        {
            var response = await _httpClient.PutAsJsonAsync(
                $"api/categories/{category.CategoryId}",
                category);

            await ApiResponse.EnsureSuccessAsync(response);
        }

        public async Task DeleteCategory(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/categories/{id}");

            await ApiResponse.EnsureSuccessAsync(response);
        }
    }
}