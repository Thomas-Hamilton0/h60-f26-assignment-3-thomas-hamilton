using THH60Services.Models.Entities;

namespace THH60Services.Interfaces;

public interface IShoppingCartRepository
{
    List<ShoppingCart> GetCarts();
    ShoppingCart? GetById(int id);
    bool Exists(int id);
    ShoppingCart? GetByCustomerId(int customerId);
    ShoppingCart Add(ShoppingCart cart);
    void Update(ShoppingCart cart);
    void Delete(int id);
}
