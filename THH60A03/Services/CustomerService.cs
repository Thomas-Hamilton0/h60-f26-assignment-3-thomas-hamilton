using System.Net;

using TH_H60_A03.Models.Entities;

namespace TH_H60_A03.Services
{
    public class CustomerService
    {
        private readonly HttpClient _httpClient;

        public CustomerService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Customer>> GetCustomers()
        {
            return await _httpClient.GetFromJsonAsync<List<Customer>>(
                "api/customers")
                ?? [];
        }

        public async Task<Customer?> GetCustomer(int id)
        {
            var response = await _httpClient.GetAsync($"api/customers/{id}");

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            await ApiResponse.EnsureSuccessAsync(response);

            return await response.Content.ReadFromJsonAsync<Customer>();
        }

        public async Task CreateCustomer(Customer customer)
        {
            var response = await _httpClient.PostAsJsonAsync(
                "api/customers",
                customer);

            await ApiResponse.EnsureSuccessAsync(response);
        }

        public async Task UpdateCustomer(Customer customer)
        {
            var response = await _httpClient.PutAsJsonAsync(
                $"api/customers/{customer.CustomerId}",
                customer);

            await ApiResponse.EnsureSuccessAsync(response);
        }

        public async Task DeleteCustomer(int id)
        {
            var response = await _httpClient.DeleteAsync(
                $"api/customers/{id}");

            await ApiResponse.EnsureSuccessAsync(response);
        }
    }
}
