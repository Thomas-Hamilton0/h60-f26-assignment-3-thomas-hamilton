namespace TH_H60_A03.Models.ViewModels;

public class UpdateStockViewModel
{
    public int ProductId { get; set; }

    public string? Description { get; set; }

    public int CurrentStock { get; set; }

    public int StockChange { get; set; }
}