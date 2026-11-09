namespace THH60Customer.Models
{
    public class CatalogViewModel
    {
        public List<CustomerProduct> Products { get; set; } = new();

        public List<CustomerCategory> Categories { get; set; } = new();

        public int? CategoryId { get; set; }

        public string? Search { get; set; }

        public int ProductCount => Products.Count;
    }
}
