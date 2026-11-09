using Microsoft.AspNetCore.Mvc;
using THH60Services.Interfaces;
using THH60Services.Models;

namespace THH60Services.Controllers;

[ApiController]
[Route("api/shoppingcarts/{cartId:int}/items")]
public class CartItemsController : ControllerBase
{
    private readonly ICartItemRepository _cartItemRepository;
    private readonly IShoppingCartRepository _shoppingCartRepository;

    public CartItemsController(
        ICartItemRepository cartItemRepository,
        IShoppingCartRepository shoppingCartRepository)
    {
        _cartItemRepository = cartItemRepository;
        _shoppingCartRepository = shoppingCartRepository;
    }

    [HttpGet]
    public IActionResult GetItems(int cartId)
    {
        if (!_shoppingCartRepository.Exists(cartId))
        {
            return NotFound(new
            {
                message = "Shopping cart not found."
            });
        }

        return Ok(_cartItemRepository.GetItems(cartId));
    }

    [HttpGet("{itemId:int}")]
    public IActionResult GetItem(int cartId, int itemId)
    {
        var item = _cartItemRepository.GetById(cartId, itemId);

        if (item == null)
        {
            return NotFound();
        }

        return Ok(item);
    }

    [HttpPost]
    public IActionResult AddItem(int cartId, AddCartItemRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var item = _cartItemRepository.Add(
                cartId,
                request.ProductId,
                request.Quantity);

            return CreatedAtAction(
                nameof(GetItem),
                new { cartId, itemId = item.CartItemId },
                item);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new
            {
                message = ex.Message
            });
        }
        catch (ArgumentOutOfRangeException ex)
        {
            return BadRequest(new
            {
                message = ex.Message
            });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new
            {
                message = ex.Message
            });
        }
    }

    [HttpPut("{itemId:int}")]
    public IActionResult UpdateItem(
        int cartId,
        int itemId,
        UpdateCartItemRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var item = _cartItemRepository.UpdateQuantity(
                cartId,
                itemId,
                request.Quantity);

            return Ok(item);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new
            {
                message = ex.Message
            });
        }
        catch (ArgumentOutOfRangeException ex)
        {
            return BadRequest(new
            {
                message = ex.Message
            });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new
            {
                message = ex.Message
            });
        }
    }

    [HttpDelete("{itemId:int}")]
    public IActionResult DeleteItem(int cartId, int itemId)
    {
        try
        {
            _cartItemRepository.Delete(cartId, itemId);

            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new
            {
                message = ex.Message
            });
        }
    }
}
