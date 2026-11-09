using TH_H60_A02.Models.Entities;

namespace TH_H60_A02.Models.ViewModels;

public class ProductViewModel
{
    public int ProductId { get; set; }

    public int ProdCatId { get; set; }

    public string? Description { get; set; }

    public string? Manufacturer { get; set; }

    public int Stock { get; set; }

    public decimal? BuyPrice { get; set; }

    public decimal? SellPrice { get; set; }

    public bool CanManagePrices { get; set; }

    public List<ProductCategory> Categories { get; set; } = new();
}