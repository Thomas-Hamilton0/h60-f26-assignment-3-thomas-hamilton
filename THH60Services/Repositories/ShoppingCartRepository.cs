using Microsoft.EntityFrameworkCore;
using THH60Services.Interfaces;
using THH60Services.Models;
using THH60Services.Models.Entities;

namespace THH60Services.Repositories;

public class ShoppingCartRepository : IShoppingCartRepository
{
    private readonly H60AssignmentDbThContext _context;

    public ShoppingCartRepository(H60AssignmentDbThContext context)
    {
        _context = context;
    }

    public List<ShoppingCart> GetCarts()
    {
        return _context.ShoppingCarts
            .Include(c => c.CartItems)
                .ThenInclude(i => i.Product)
            .OrderBy(c => c.CustomerId)
            .ToList();
    }

    public ShoppingCart? GetById(int id)
    {
        return _context.ShoppingCarts
            .Include(c => c.CartItems)
                .ThenInclude(i => i.Product)
            .FirstOrDefault(c => c.ShoppingCartId == id);
    }

    public bool Exists(int id)
    {
        return _context.ShoppingCarts
            .Any(c => c.ShoppingCartId == id);
    }

    public ShoppingCart? GetByCustomerId(int customerId)
    {
        return _context.ShoppingCarts
            .Include(c => c.CartItems)
                .ThenInclude(i => i.Product)
            .FirstOrDefault(c => c.CustomerId == customerId);
    }

    public ShoppingCart Add(ShoppingCart cart)
    {
        _context.ShoppingCarts.Add(cart);
        _context.SaveChanges();

        return cart;
    }

    public void Update(ShoppingCart cart)
    {
        var existing = _context.ShoppingCarts.Find(cart.ShoppingCartId);

        if (existing == null)
        {
            return;
        }

        existing.CustomerId = cart.CustomerId;

        _context.SaveChanges();
    }

    public void Delete(int id)
    {
        var cart = _context.ShoppingCarts
            .Include(c => c.CartItems)
            .FirstOrDefault(c => c.ShoppingCartId == id);

        if (cart == null)
        {
            return;
        }

        if (cart.CartItems.Any())
        {
            throw new InvalidOperationException(
                "Cannot delete a shopping cart that contains items.");
        }

        _context.ShoppingCarts.Remove(cart);
        _context.SaveChanges();
    }
}
