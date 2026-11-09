namespace TH_H60_A03.Models.ViewModels

{
    public class UpdatePricesViewModel
    {
        public int ProductId { get; set; }

        public string BuyPrice { get; set; } = "";

        public string SellPrice { get; set; } = "";

        public string? Description { get; set; }
    }
}