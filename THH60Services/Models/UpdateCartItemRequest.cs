using System.ComponentModel.DataAnnotations;

namespace THH60Services.Models;

public class UpdateCartItemRequest
{
    [Range(1, int.MaxValue)]
    public int Quantity { get; set; }
}
