using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace THH60Services.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    CustomerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    EmailAddress = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Province = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.CustomerId);
                });

            migrationBuilder.CreateTable(
                name: "ProductCategory",
                columns: table => new
                {
                    CategoryID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProdCat = table.Column<string>(type: "varchar(60)", unicode: false, maxLength: 60, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductCategory", x => x.CategoryID);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    OrderId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateFulfilled = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Total = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Taxes = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.OrderId);
                    table.ForeignKey(
                        name: "FK_Orders_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ShoppingCarts",
                columns: table => new
                {
                    ShoppingCartId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShoppingCarts", x => x.ShoppingCartId);
                    table.ForeignKey(
                        name: "FK_ShoppingCarts_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    ProductID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProdCatId = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "varchar(80)", unicode: false, maxLength: 80, nullable: true),
                    Manufacturer = table.Column<string>(type: "varchar(80)", unicode: false, maxLength: 80, nullable: true),
                    Stock = table.Column<int>(type: "int", nullable: false),
                    BuyPrice = table.Column<decimal>(type: "numeric(8,2)", nullable: true),
                    SellPrice = table.Column<decimal>(type: "numeric(8,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.ProductID);
                    table.ForeignKey(
                        name: "FK_Product_ProductCategory",
                        column: x => x.ProdCatId,
                        principalTable: "ProductCategory",
                        principalColumn: "CategoryID");
                });

            migrationBuilder.CreateTable(
                name: "CartItems",
                columns: table => new
                {
                    CartItemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ShoppingCartId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartItems", x => x.CartItemId);
                    table.ForeignKey(
                        name: "FK_CartItems_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "ProductID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CartItems_ShoppingCarts_ShoppingCartId",
                        column: x => x.ShoppingCartId,
                        principalTable: "ShoppingCarts",
                        principalColumn: "ShoppingCartId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderItems",
                columns: table => new
                {
                    OrderItemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItems", x => x.OrderItemId);
                    table.ForeignKey(
                        name: "FK_OrderItems_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderItems_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "ProductID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "CustomerId", "EmailAddress", "FirstName", "LastName", "Password", "PhoneNumber", "Province" },
                values: new object[,]
                {
                    { 1, "thomas@heritagereptiles.ca", "Thomas", "Hamilton", "Password123", "5145550001", "Quebec" },
                    { 2, "alice@heritagereptiles.ca", "Alice", "Smith", "Password123", "4165550002", "Ontario" },
                    { 3, "bob@heritagereptiles.ca", "Bob", "Johnson", "Password123", "5065550003", "New Brunswick" }
                });

            migrationBuilder.InsertData(
                table: "ProductCategory",
                columns: new[] { "CategoryID", "ProdCat" },
                values: new object[,]
                {
                    { 1, "Snakes" },
                    { 2, "Lizards" },
                    { 3, "Turtles & Tortoises" },
                    { 4, "Terrariums" },
                    { 5, "Heating & Lighting" },
                    { 6, "Food & Supplements" }
                });

            migrationBuilder.InsertData(
                table: "Orders",
                columns: new[] { "OrderId", "CustomerId", "DateCreated", "DateFulfilled", "Taxes", "Total" },
                values: new object[] { 1, 1, new DateTime(2026, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 21.45m, 164.97m });

            migrationBuilder.InsertData(
                table: "Product",
                columns: new[] { "ProductID", "BuyPrice", "Description", "Manufacturer", "ProdCatId", "SellPrice", "Stock" },
                values: new object[,]
                {
                    { 1, 75.00m, "Ball Python", "Reptiles by Mack", 1, 129.99m, 12 },
                    { 2, 45.00m, "Corn Snake", "Underground Reptiles", 1, 79.99m, 10 },
                    { 3, 55.00m, "California Kingsnake", "XYZReptiles", 1, 99.99m, 8 },
                    { 4, 50.00m, "Milk Snake", "Backwater Reptiles", 1, 89.99m, 7 },
                    { 5, 60.00m, "Bearded Dragon", "Reptiles by Mack", 2, 119.99m, 15 },
                    { 6, 35.00m, "Leopard Gecko", "Underground Reptiles", 2, 69.99m, 20 },
                    { 7, 40.00m, "Crested Gecko", "Pangea Reptile", 2, 79.99m, 12 },
                    { 8, 150.00m, "Blue Tongued Skink", "XYZReptiles", 2, 229.99m, 5 },
                    { 9, 20.00m, "Red Eared Slider", "Backwater Reptiles", 3, 44.99m, 10 },
                    { 10, 25.00m, "Painted Turtle", "Underground Reptiles", 3, 49.99m, 8 },
                    { 11, 90.00m, "Russian Tortoise", "Tortoise Town", 3, 149.99m, 4 },
                    { 12, 100.00m, "Greek Tortoise", "Tortoise Town", 3, 169.99m, 3 },
                    { 13, 40.00m, "Small Glass Terrarium", "Exo Terra", 4, 79.99m, 15 },
                    { 14, 70.00m, "Naturalistic Terrarium", "Zoo Med", 4, 129.99m, 10 },
                    { 15, 120.00m, "Large Glass Terrarium", "Exo Terra", 4, 229.99m, 5 },
                    { 16, 80.00m, "Bioactive Starter Kit", "Joshs Frogs", 4, 149.99m, 6 },
                    { 17, 10.00m, "Repti Basking Spot Lamp", "Zoo Med", 5, 19.99m, 25 },
                    { 18, 15.00m, "Ceramic Heat Emitter", "Flukers", 5, 29.99m, 20 },
                    { 19, 12.00m, "ReptiSun UVB Bulb", "Zoo Med", 5, 24.99m, 18 },
                    { 20, 25.00m, "Herpstat Thermostat", "Spyder Robotics", 5, 49.99m, 12 },
                    { 21, 2.50m, "Mealworms", "Flukers", 6, 4.99m, 100 },
                    { 22, 3.00m, "Crickets", "Premium Crickets", 6, 5.99m, 150 },
                    { 23, 4.00m, "Repti Calcium", "Zoo Med", 6, 8.99m, 40 },
                    { 24, 5.00m, "Calcium Plus", "Repashy", 6, 9.99m, 35 }
                });

            migrationBuilder.InsertData(
                table: "ShoppingCarts",
                columns: new[] { "ShoppingCartId", "CustomerId" },
                values: new object[] { 1, 2 });

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_ProductId",
                table: "CartItems",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_ShoppingCartId",
                table: "CartItems",
                column: "ShoppingCartId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_OrderId",
                table: "OrderItems",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_ProductId",
                table: "OrderItems",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CustomerId",
                table: "Orders",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Product_ProdCatId",
                table: "Product",
                column: "ProdCatId");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingCarts_CustomerId",
                table: "ShoppingCarts",
                column: "CustomerId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CartItems");

            migrationBuilder.DropTable(
                name: "OrderItems");

            migrationBuilder.DropTable(
                name: "ShoppingCarts");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Product");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "ProductCategory");
        }
    }
}
