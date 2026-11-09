namespace TH_H60_A03.Models.Entities;

public class ShoppingCart
{
    public int ShoppingCartId { get; set; }

    public int CustomerId { get; set; }

    public virtual Customer Customer { get; set; } = null!;

    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
}