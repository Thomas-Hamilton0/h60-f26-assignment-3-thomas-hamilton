using THH60Services.Models.Entities;

namespace THH60Services.Interfaces;

public interface IProductRepository
{
    List<Product> GetProducts();
    List<Product> GetAll();
    Product? GetById(int id);
    void Add(Product product);
    void Update(Product product);
    void Delete(int id);
    void UpdateStock(int productId, int stockChange);
    void UpdatePrices(int productId, decimal buyPrice, decimal sellPrice);
    List<Product> GetProductsByCategory();
    List<Product> GetProductsByCategoryId(int categoryId);
    Product? GetDetails(int id);
}