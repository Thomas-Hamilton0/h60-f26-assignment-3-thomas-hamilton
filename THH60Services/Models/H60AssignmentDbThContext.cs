using Microsoft.EntityFrameworkCore;
using THH60Services.Models.Entities;

namespace THH60Services.Models;

public partial class H60AssignmentDbThContext : DbContext
{
    public H60AssignmentDbThContext()
    {
    }

    public H60AssignmentDbThContext(DbContextOptions<H60AssignmentDbThContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductCategory> ProductCategories { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<ShoppingCart> ShoppingCarts { get; set; }

    public virtual DbSet<CartItem> CartItems { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderItem> OrderItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("Product");

            entity.HasIndex(e => e.ProdCatId, "IX_Product_ProdCatId");

            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.BuyPrice).HasColumnType("numeric(8, 2)");
            entity.Property(e => e.Description)
                .HasMaxLength(80)
                .IsUnicode(false);
            entity.Property(e => e.Manufacturer)
                .HasMaxLength(80)
                .IsUnicode(false);
            entity.Property(e => e.SellPrice).HasColumnType("numeric(8, 2)");

            entity.HasOne(d => d.ProdCat).WithMany(p => p.Products)
                .HasForeignKey(d => d.ProdCatId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Product_ProductCategory");
        });

        modelBuilder.Entity<ProductCategory>(entity =>
        {
            entity.HasKey(e => e.CategoryId);

            entity.ToTable("ProductCategory");

            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.ProdCat)
                .HasMaxLength(60)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.CustomerId);

            entity.Property(e => e.FirstName)
                .HasMaxLength(50);

            entity.Property(e => e.LastName)
                .HasMaxLength(50);

            entity.Property(e => e.EmailAddress)
                .HasMaxLength(100);

            entity.Property(e => e.IdentityUserId)
                .HasMaxLength(450);

            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(20);

            entity.Property(e => e.Province)
                .HasMaxLength(20);
        });

        modelBuilder.Entity<ShoppingCart>(entity =>
        {
            entity.HasKey(e => e.ShoppingCartId);

            entity.HasOne(e => e.Customer)
                .WithOne(e => e.ShoppingCart)
                .HasForeignKey<ShoppingCart>(e => e.CustomerId);
        });

        modelBuilder.Entity<CartItem>(entity =>
        {
            entity.HasKey(e => e.CartItemId);

            entity.HasIndex(e => new { e.ShoppingCartId, e.ProductId })
                .IsUnique();

            entity.HasOne(e => e.ShoppingCart)
                .WithMany(e => e.CartItems)
                .HasForeignKey(e => e.ShoppingCartId);

            entity.HasOne(e => e.Product)
                .WithMany(e => e.CartItems)
                .HasForeignKey(e => e.ProductId);
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId);

            entity.HasOne(e => e.Customer)
                .WithMany(e => e.Orders)
                .HasForeignKey(e => e.CustomerId);
        });

        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.HasKey(e => e.OrderItemId);

            entity.HasOne(e => e.Order)
                .WithMany(e => e.OrderItems)
                .HasForeignKey(e => e.OrderId);

            entity.HasOne(e => e.Product)
                .WithMany(e => e.OrderItems)
                .HasForeignKey(e => e.ProductId);
        });


        modelBuilder.Entity<ProductCategory>().HasData(
            new ProductCategory { CategoryId = 1, ProdCat = "Snakes" },
            new ProductCategory { CategoryId = 2, ProdCat = "Lizards" },
            new ProductCategory { CategoryId = 3, ProdCat = "Turtles & Tortoises" },
            new ProductCategory { CategoryId = 4, ProdCat = "Terrariums" },
            new ProductCategory { CategoryId = 5, ProdCat = "Heating & Lighting" },
            new ProductCategory { CategoryId = 6, ProdCat = "Food & Supplements" }
        );

        modelBuilder.Entity<Product>().HasData(
            new Product { ProductId = 1, ProdCatId = 1, Description = "Ball Python", Manufacturer = "Reptiles by Mack", Stock = 12, BuyPrice = 75.00m, SellPrice = 129.99m },
            new Product { ProductId = 2, ProdCatId = 1, Description = "Corn Snake", Manufacturer = "Underground Reptiles", Stock = 10, BuyPrice = 45.00m, SellPrice = 79.99m },
            new Product { ProductId = 3, ProdCatId = 1, Description = "California Kingsnake", Manufacturer = "XYZReptiles", Stock = 8, BuyPrice = 55.00m, SellPrice = 99.99m },
            new Product { ProductId = 4, ProdCatId = 1, Description = "Milk Snake", Manufacturer = "Backwater Reptiles", Stock = 7, BuyPrice = 50.00m, SellPrice = 89.99m },
            new Product { ProductId = 5, ProdCatId = 2, Description = "Bearded Dragon", Manufacturer = "Reptiles by Mack", Stock = 15, BuyPrice = 60.00m, SellPrice = 119.99m },
            new Product { ProductId = 6, ProdCatId = 2, Description = "Leopard Gecko", Manufacturer = "Underground Reptiles", Stock = 20, BuyPrice = 35.00m, SellPrice = 69.99m },
            new Product { ProductId = 7, ProdCatId = 2, Description = "Crested Gecko", Manufacturer = "Pangea Reptile", Stock = 12, BuyPrice = 40.00m, SellPrice = 79.99m },
            new Product { ProductId = 8, ProdCatId = 2, Description = "Blue Tongued Skink", Manufacturer = "XYZReptiles", Stock = 5, BuyPrice = 150.00m, SellPrice = 229.99m },
            new Product { ProductId = 9, ProdCatId = 3, Description = "Red Eared Slider", Manufacturer = "Backwater Reptiles", Stock = 10, BuyPrice = 20.00m, SellPrice = 44.99m },
            new Product { ProductId = 10, ProdCatId = 3, Description = "Painted Turtle", Manufacturer = "Underground Reptiles", Stock = 8, BuyPrice = 25.00m, SellPrice = 49.99m },
            new Product { ProductId = 11, ProdCatId = 3, Description = "Russian Tortoise", Manufacturer = "Tortoise Town", Stock = 4, BuyPrice = 90.00m, SellPrice = 149.99m },
            new Product { ProductId = 12, ProdCatId = 3, Description = "Greek Tortoise", Manufacturer = "Tortoise Town", Stock = 3, BuyPrice = 100.00m, SellPrice = 169.99m },
            new Product { ProductId = 13, ProdCatId = 4, Description = "Small Glass Terrarium", Manufacturer = "Exo Terra", Stock = 15, BuyPrice = 40.00m, SellPrice = 79.99m },
            new Product { ProductId = 14, ProdCatId = 4, Description = "Naturalistic Terrarium", Manufacturer = "Zoo Med", Stock = 10, BuyPrice = 70.00m, SellPrice = 129.99m },
            new Product { ProductId = 15, ProdCatId = 4, Description = "Large Glass Terrarium", Manufacturer = "Exo Terra", Stock = 5, BuyPrice = 120.00m, SellPrice = 229.99m },
            new Product { ProductId = 16, ProdCatId = 4, Description = "Bioactive Starter Kit", Manufacturer = "Joshs Frogs", Stock = 6, BuyPrice = 80.00m, SellPrice = 149.99m },
            new Product { ProductId = 17, ProdCatId = 5, Description = "Repti Basking Spot Lamp", Manufacturer = "Zoo Med", Stock = 25, BuyPrice = 10.00m, SellPrice = 19.99m },
            new Product { ProductId = 18, ProdCatId = 5, Description = "Ceramic Heat Emitter", Manufacturer = "Flukers", Stock = 20, BuyPrice = 15.00m, SellPrice = 29.99m },
            new Product { ProductId = 19, ProdCatId = 5, Description = "ReptiSun UVB Bulb", Manufacturer = "Zoo Med", Stock = 18, BuyPrice = 12.00m, SellPrice = 24.99m },
            new Product { ProductId = 20, ProdCatId = 5, Description = "Herpstat Thermostat", Manufacturer = "Spyder Robotics", Stock = 12, BuyPrice = 25.00m, SellPrice = 49.99m },
            new Product { ProductId = 21, ProdCatId = 6, Description = "Mealworms", Manufacturer = "Flukers", Stock = 100, BuyPrice = 2.50m, SellPrice = 4.99m },
            new Product { ProductId = 22, ProdCatId = 6, Description = "Crickets", Manufacturer = "Premium Crickets", Stock = 150, BuyPrice = 3.00m, SellPrice = 5.99m },
            new Product { ProductId = 23, ProdCatId = 6, Description = "Repti Calcium", Manufacturer = "Zoo Med", Stock = 40, BuyPrice = 4.00m, SellPrice = 8.99m },
            new Product { ProductId = 24, ProdCatId = 6, Description = "Calcium Plus", Manufacturer = "Repashy", Stock = 35, BuyPrice = 5.00m, SellPrice = 9.99m }
        );

        modelBuilder.Entity<Customer>().HasData(

            new Customer
            {
                CustomerId = 1,
                FirstName = "Thomas",
                LastName = "Hamilton",
                EmailAddress = "thomas@heritagereptiles.ca",
                IdentityUserId = "11111111-1111-1111-1111-111111111111",
                PhoneNumber = "5145550001",
                Province = "Quebec"
            },

            new Customer
            {
                CustomerId = 2,
                FirstName = "Alice",
                LastName = "Smith",
                EmailAddress = "alice@heritagereptiles.ca",
                IdentityUserId = "22222222-2222-2222-2222-222222222222",
                PhoneNumber = "4165550002",
                Province = "Ontario"
            },

            new Customer
            {
                CustomerId = 3,
                FirstName = "Bob",
                LastName = "Johnson",
                EmailAddress = "bob@heritagereptiles.ca",
                IdentityUserId = "33333333-3333-3333-3333-333333333333",
                PhoneNumber = "5065550003",
                Province = "New Brunswick"
            }
        );

        modelBuilder.Entity<ShoppingCart>().HasData(

            new ShoppingCart
            {
                ShoppingCartId = 1,
                CustomerId = 2
            }

        );

        modelBuilder.Entity<Order>().HasData(

            new Order
            {
                OrderId = 1,
                CustomerId = 1,
                DateCreated = new DateTime(2026, 1, 10),
                DateFulfilled = new DateTime(2026, 1, 15),
                Total = 164.97m,
                Taxes = 21.45m
            }

        );





        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
