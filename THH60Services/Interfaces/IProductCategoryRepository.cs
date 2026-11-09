using THH60Services.Models.Entities;

namespace THH60Services.Interfaces;

public interface IProductCategoryRepository
{
    List<ProductCategory> GetCategories();
    ProductCategory? GetById(int id);
    void Add(ProductCategory category);
    void Update(ProductCategory category);
    void Delete(int id);
}