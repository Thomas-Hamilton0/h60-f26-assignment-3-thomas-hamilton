using Microsoft.AspNetCore.Mvc;
using THH60Services.Interfaces;
using THH60Services.Models.Entities;

namespace THH60Services.Controllers;

[ApiController]
[Route("api/customers")]
public class CustomersController : ControllerBase
{
    private readonly ICustomerRepository _customerRepository;

    public CustomersController(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    [HttpGet]
    public IActionResult GetCustomers()
    {
        return Ok(_customerRepository.GetCustomers());
    }

    [HttpGet("{id}")]
    public IActionResult GetCustomer(int id)
    {
        var customer = _customerRepository.GetById(id);

        if (customer == null)
        {
            return NotFound();
        }

        return Ok(customer);
    }

    [HttpPost]
    public IActionResult CreateCustomer(Customer customer)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (IsDuplicate(customer))
        {
            return BadRequest(new
            {
                message = "A customer with this email address or sign-in account already exists."
            });
        }

        _customerRepository.Add(customer);

        return CreatedAtAction(
            nameof(GetCustomer),
            new { id = customer.CustomerId },
            customer);
    }

    [HttpPut("{id}")]
    public IActionResult UpdateCustomer(int id, Customer customer)
    {
        if (id != customer.CustomerId)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var existing = _customerRepository.GetById(id);

        if (existing == null)
        {
            return NotFound();
        }

        if (IsDuplicate(customer, id))
        {
            return BadRequest(new
            {
                message = "Another customer already uses this email address or sign-in account."
            });
        }

        _customerRepository.Update(customer);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteCustomer(int id)
    {
        var existing = _customerRepository.GetById(id);

        if (existing == null)
        {
            return NotFound();
        }

        try
        {
            _customerRepository.Delete(id);

            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    private bool IsDuplicate(Customer customer, int? currentCustomerId = null)
    {
        return _customerRepository
            .GetCustomers()
            .Any(existing =>
                existing.CustomerId != (currentCustomerId ?? 0) &&
                (string.Equals(
                     existing.EmailAddress,
                     customer.EmailAddress,
                     StringComparison.OrdinalIgnoreCase) ||
                 (!string.IsNullOrWhiteSpace(customer.IdentityUserId) &&
                  string.Equals(
                      existing.IdentityUserId,
                      customer.IdentityUserId,
                      StringComparison.OrdinalIgnoreCase))));
    }
}