using Microsoft.EntityFrameworkCore;
using THH60Services.Interfaces;
using THH60Services.Models;
using THH60Services.Models.Entities;

namespace THH60Services.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly H60AssignmentDbThContext _context;

    public CustomerRepository(H60AssignmentDbThContext context)
    {
        _context = context;
    }

    public List<Customer> GetCustomers()
    {
        return _context.Customers
            .OrderBy(c => c.LastName)
            .ThenBy(c => c.FirstName)
            .ToList();
    }

    public Customer? GetById(int id)
    {
        return _context.Customers.Find(id);
    }

    public void Add(Customer customer)
    {
        _context.Customers.Add(customer);
        _context.SaveChanges();
    }

    public void Update(Customer customer)
    {
        var existing = _context.Customers.Find(customer.CustomerId);

        if (existing == null)
        {
            return;
        }

        _context.Entry(existing).CurrentValues.SetValues(customer);
        _context.SaveChanges();
    }

    public void Delete(int id)
    {
        var customer = _context.Customers
            .Include(c => c.ShoppingCart)
            .Include(c => c.Orders)
            .FirstOrDefault(c => c.CustomerId == id);

        if (customer == null)
        {
            return;
        }

        if (customer.ShoppingCart != null || customer.Orders.Any())
        {
            throw new InvalidOperationException(
                "Cannot delete a customer with a shopping cart or orders.");
        }

        _context.Customers.Remove(customer);
        _context.SaveChanges();
    }
}