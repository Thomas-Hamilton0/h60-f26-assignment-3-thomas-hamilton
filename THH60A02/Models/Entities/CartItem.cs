namespace TH_H60_A02.Models.Entities;

public class CartItem
{
    public int CartItemId { get; set; }

    public int ShoppingCartId { get; set; }

    public int ProductId { get; set; }

    public int Quantity { get; set; }

    public decimal Price { get; set; }

    public virtual ShoppingCart ShoppingCart { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}