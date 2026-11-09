using TH_H60_A03.Models.Entities;

namespace TH_H60_A03.Models.ViewModels
{
    public class ProductCategoryProductsViewModel
    {
        public int CategoryId { get; set; }

        public List<ProductCategory> Categories { get; set; } = new();

        public List<Product> Products { get; set; } = new();
    }
}
