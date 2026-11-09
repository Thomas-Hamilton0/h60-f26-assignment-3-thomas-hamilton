using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace THH60Services.Migrations
{
    /// <inheritdoc />
    public partial class AddCartItemUniqueIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CartItems_ShoppingCartId",
                table: "CartItems");

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_ShoppingCartId_ProductId",
                table: "CartItems",
                columns: new[] { "ShoppingCartId", "ProductId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CartItems_ShoppingCartId_ProductId",
                table: "CartItems");

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_ShoppingCartId",
                table: "CartItems",
                column: "ShoppingCartId");
        }
    }
}
