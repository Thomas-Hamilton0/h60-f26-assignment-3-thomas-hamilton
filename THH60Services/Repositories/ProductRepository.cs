using Microsoft.EntityFrameworkCore;
using THH60Services.Interfaces;
using THH60Services.Models;
using THH60Services.Models.Entities;

namespace THH60Services.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly H60AssignmentDbThContext _context;

        public ProductRepository(H60AssignmentDbThContext context)
        {
            _context = context;
        }

        public List<Product> GetAll()
        {
            return _context.Products.ToList();
        }

        public Product? GetById(int id)
        {
            return _context.Products.Find(id);
        }

        public void Add(Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();
        }

        public void Update(Product product)
        {
            var existing = _context.Products.Find(product.ProductId);

            if (existing == null)
            {
                return;
            }

            _context.Entry(existing).CurrentValues.SetValues(product);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var product = _context.Products.Find(id);

            if (product != null)
            {
                _context.Products.Remove(product);
                _context.SaveChanges();
            }
        }

        public List<Product> GetProducts()
        {
            return _context.Products
                    .OrderBy(p => p.Description)
                    .ToList();
        }

        public void UpdateStock(int productId, int stockChange)
        {
            Product? product = _context.Products.Find(productId);

            if (product == null)
            {
                throw new Exception("Product not found.");
            }

            int newStock = product.Stock + stockChange;

            if (newStock < 0)
            {
                throw new Exception("Stock cannot be less than zero.");
            }

            product.Stock = newStock;

            _context.SaveChanges();
        }

        public void UpdatePrices(int productId, decimal buyPrice, decimal sellPrice)
        {
            Product? product = _context.Products.Find(productId);

            if (product == null)
            {
                throw new Exception("Product not found.");
            }

            buyPrice = Math.Round(buyPrice, 2);
            sellPrice = Math.Round(sellPrice, 2);

            if (buyPrice < 0 || sellPrice < 0)
            {
                throw new ArgumentException("Prices cannot be negative.");
            }

            if (sellPrice < buyPrice)
            {
                throw new Exception("Sell price cannot be less than buy price.");
            }

            product.BuyPrice = buyPrice;
            product.SellPrice = sellPrice;

            _context.SaveChanges();
        }
        public List<Product> GetProductsByCategory()
        {
            return _context.Products
                    .Include(p => p.ProdCat)
                    .OrderBy(p => p.ProdCat!.ProdCat)
                    .ThenBy(p => p.Description)
                    .ToList();
        }

        public List<Product> GetProductsByCategoryId(int categoryId)
        {
            return _context.Products
                    .Where(p => p.ProdCatId == categoryId)
                    .OrderBy(p => p.Description)
                    .ToList();
        }

        public Product? GetDetails(int id)
        {
            return _context.Products
                    .Include(p => p.ProdCat)
                    .FirstOrDefault(p => p.ProductId == id);
        }
    }
}
