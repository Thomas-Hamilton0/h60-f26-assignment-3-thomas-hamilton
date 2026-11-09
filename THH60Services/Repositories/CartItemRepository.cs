using Microsoft.EntityFrameworkCore;
using THH60Services.Interfaces;
using THH60Services.Models;
using THH60Services.Models.Entities;

namespace THH60Services.Repositories;

public class CartItemRepository : ICartItemRepository
{
    private readonly H60AssignmentDbThContext _context;

    public CartItemRepository(H60AssignmentDbThContext context)
    {
        _context = context;
    }

    public List<CartItem> GetItems(int cartId)
    {
        return _context.CartItems
            .Where(i => i.ShoppingCartId == cartId)
            .Include(i => i.Product)
            .OrderBy(i => i.Product.Description)
            .ToList();
    }

    public CartItem? GetById(int cartId, int itemId)
    {
        return _context.CartItems
            .Include(i => i.Product)
            .FirstOrDefault(i =>
                i.CartItemId == itemId &&
                i.ShoppingCartId == cartId);
    }

    public CartItem Add(int cartId, int productId, int quantity)
    {
        if (quantity <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(quantity),
                "Quantity must be greater than zero.");
        }

        if (_context.ShoppingCarts.Find(cartId) == null)
        {
            throw new KeyNotFoundException("Shopping cart not found.");
        }

        var product = _context.Products
            .AsNoTracking()
            .FirstOrDefault(p => p.ProductId == productId);

        if (product == null)
        {
            throw new KeyNotFoundException("Product not found.");
        }

        const int maxAttempts = 3;

        for (int attempt = 1; attempt <= maxAttempts; attempt++)
        {
            using var transaction = _context.Database.BeginTransaction();

            try
            {
                var existing = _context.CartItems
                    .FirstOrDefault(i =>
                        i.ShoppingCartId == cartId &&
                        i.ProductId == productId);

                if (existing != null)
                {
                    ReserveStock(productId, quantity);

                    _context.Database.ExecuteSqlInterpolated(
                        $@"UPDATE CartItems
                           SET Quantity = Quantity + {quantity}
                           WHERE CartItemId = {existing.CartItemId}");

                    _context.Entry(existing).Reload();
                    transaction.Commit();

                    return existing;
                }

                ReserveStock(productId, quantity);

                var item = new CartItem
                {
                    ShoppingCartId = cartId,
                    ProductId = productId,
                    Quantity = quantity,
                    Price = product.SellPrice ?? 0m
                };

                _context.CartItems.Add(item);
                _context.SaveChanges();
                transaction.Commit();

                return item;
            }
            catch (DbUpdateException) when (attempt < maxAttempts)
            {
                // A concurrent request inserted the same cart/product line.
                // Roll back, clear the failed insert and retry as a merge.
                transaction.Rollback();
                _context.ChangeTracker.Clear();
            }
        }

        throw new InvalidOperationException(
            "The item could not be added to the cart. Please try again.");
    }

    public CartItem UpdateQuantity(int cartId, int itemId, int quantity)
    {
        if (quantity <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(quantity),
                "Quantity must be greater than zero.");
        }

        using var transaction = _context.Database.BeginTransaction();

        var item = _context.CartItems
            .FirstOrDefault(i =>
                i.CartItemId == itemId &&
                i.ShoppingCartId == cartId);

        if (item == null)
        {
            throw new KeyNotFoundException("Cart item not found.");
        }

        int change = quantity - item.Quantity;

        if (change > 0)
        {
            ReserveStock(item.ProductId, change);
        }
        else if (change < 0)
        {
            ReleaseStock(item.ProductId, -change);
        }

        item.Quantity = quantity;

        _context.SaveChanges();
        transaction.Commit();

        return item;
    }

    public void Delete(int cartId, int itemId)
    {
        using var transaction = _context.Database.BeginTransaction();

        var item = _context.CartItems
            .FirstOrDefault(i =>
                i.CartItemId == itemId &&
                i.ShoppingCartId == cartId);

        if (item == null)
        {
            throw new KeyNotFoundException("Cart item not found.");
        }

        ReleaseStock(item.ProductId, item.Quantity);

        _context.CartItems.Remove(item);
        _context.SaveChanges();
        transaction.Commit();
    }

    private void ReserveStock(int productId, int quantity)
    {
        int affected = _context.Database.ExecuteSqlInterpolated(
            $@"UPDATE Product
               SET Stock = Stock - {quantity}
               WHERE ProductID = {productId}
                 AND Stock >= {quantity}");

        if (affected == 0)
        {
            throw new InvalidOperationException(
                "There is not enough stock for this product.");
        }
    }

    private void ReleaseStock(int productId, int quantity)
    {
        _context.Database.ExecuteSqlInterpolated(
            $@"UPDATE Product
               SET Stock = Stock + {quantity}
               WHERE ProductID = {productId}");
    }
}
