namespace THH60Customer.Models
{
    public class CustomerProduct
    {
        public int ProductId { get; set; }

        public int ProdCatId { get; set; }

        public string Description { get; set; } = "";

        public string? Manufacturer { get; set; }

        public int Stock { get; set; }

        public decimal? SellPrice { get; set; }

        public string CategoryName { get; set; } = "";

        public bool InStock => Stock > 0;
    }
}
