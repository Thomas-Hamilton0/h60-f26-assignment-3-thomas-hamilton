using THH60Services.Models.Entities;

namespace THH60Services.Interfaces;

public interface ICustomerRepository
{
    List<Customer> GetCustomers();
    Customer? GetById(int id);
    void Add(Customer customer);
    void Update(Customer customer);
    void Delete(int id);
}