using System.ComponentModel.DataAnnotations;

namespace THH60Services.Models;

public class ShoppingCartRequest
{
    [Range(1, int.MaxValue)]
    public int CustomerId { get; set; }
}
