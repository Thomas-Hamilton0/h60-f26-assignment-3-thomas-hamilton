using THH60Services.Models.Entities;

namespace THH60Services.Interfaces;

public interface ICartItemRepository
{
    List<CartItem> GetItems(int cartId);
    CartItem? GetById(int cartId, int itemId);
    CartItem Add(int cartId, int productId, int quantity);
    CartItem UpdateQuantity(int cartId, int itemId, int quantity);
    void Delete(int cartId, int itemId);
}
