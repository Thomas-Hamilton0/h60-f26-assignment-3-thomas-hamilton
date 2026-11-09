using Microsoft.AspNetCore.Mvc;
using THH60Services.Interfaces;
using THH60Services.Models;
using THH60Services.Models.Entities;

namespace THH60Services.Controllers;

[ApiController]
[Route("api/shoppingcarts")]
public class ShoppingCartsController : ControllerBase
{
    private readonly IShoppingCartRepository _shoppingCartRepository;
    private readonly ICustomerRepository _customerRepository;

    public ShoppingCartsController(
        IShoppingCartRepository shoppingCartRepository,
        ICustomerRepository customerRepository)
    {
        _shoppingCartRepository = shoppingCartRepository;
        _customerRepository = customerRepository;
    }

    [HttpGet]
    public IActionResult GetCarts()
    {
        return Ok(_shoppingCartRepository.GetCarts());
    }

    [HttpGet("{id:int}")]
    public IActionResult GetCart(int id)
    {
        var cart = _shoppingCartRepository.GetById(id);

        if (cart == null)
        {
            return NotFound();
        }

        return Ok(cart);
    }

    [HttpGet("customer/{customerId:int}")]
    public IActionResult GetCartByCustomer(int customerId)
    {
        var cart = _shoppingCartRepository.GetByCustomerId(customerId);

        if (cart == null)
        {
            return NotFound();
        }

        return Ok(cart);
    }

    [HttpPost]
    public IActionResult CreateCart(ShoppingCartRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (_customerRepository.GetById(request.CustomerId) == null)
        {
            return BadRequest(new
            {
                message = "Customer does not exist."
            });
        }

        if (_shoppingCartRepository.GetByCustomerId(request.CustomerId) != null)
        {
            return Conflict(new
            {
                message = "This customer already has a shopping cart."
            });
        }

        var cart = new ShoppingCart
        {
            CustomerId = request.CustomerId
        };

        var created = _shoppingCartRepository.Add(cart);

        return CreatedAtAction(
            nameof(GetCart),
            new { id = created.ShoppingCartId },
            created);
    }

    [HttpPut("{id:int}")]
    public IActionResult UpdateCart(int id, ShoppingCartRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (_shoppingCartRepository.GetById(id) == null)
        {
            return NotFound();
        }

        if (_customerRepository.GetById(request.CustomerId) == null)
        {
            return BadRequest(new
            {
                message = "Customer does not exist."
            });
        }

        var existing = _shoppingCartRepository.GetByCustomerId(request.CustomerId);

        if (existing != null && existing.ShoppingCartId != id)
        {
            return Conflict(new
            {
                message = "This customer already has a shopping cart."
            });
        }

        var cart = new ShoppingCart
        {
            ShoppingCartId = id,
            CustomerId = request.CustomerId
        };

        _shoppingCartRepository.Update(cart);

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public IActionResult DeleteCart(int id)
    {
        if (_shoppingCartRepository.GetById(id) == null)
        {
            return NotFound();
        }

        try
        {
            _shoppingCartRepository.Delete(id);

            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new
            {
                message = ex.Message
            });
        }
    }
}
