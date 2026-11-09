using THH60Services.Interfaces;
using THH60Services.Models;
using THH60Services.Models.Entities;

namespace THH60Services.Repositories
{
    public class ProductCategoryRepository : IProductCategoryRepository
    {
        private readonly H60AssignmentDbThContext _context;

        public ProductCategoryRepository(H60AssignmentDbThContext context)
        {
            _context = context;
        }

        public List<ProductCategory> GetAll()
        {
            return _context.ProductCategories.ToList();
        }

        public ProductCategory? GetById(int id)
        {
            return _context.ProductCategories.Find(id);
        }

        public void Add(ProductCategory category)
        {
            _context.ProductCategories.Add(category);
            _context.SaveChanges();
        }

        public void Update(ProductCategory category)
        {
            var existing = _context.ProductCategories.Find(category.CategoryId);

            if (existing == null)
            {
                return;
            }

            _context.Entry(existing).CurrentValues.SetValues(category);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var category = _context.ProductCategories.Find(id);

            if (category != null)
            {
                _context.ProductCategories.Remove(category);
                _context.SaveChanges();
            }
        }

        public List<ProductCategory> GetCategories()
        {
            return _context.ProductCategories
                    .OrderBy(c => c.ProdCat)
                    .ToList();
        }
    }
}
