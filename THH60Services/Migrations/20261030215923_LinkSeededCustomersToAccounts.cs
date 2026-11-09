using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace THH60Services.Migrations
{
    /// <inheritdoc />
    public partial class LinkSeededCustomersToAccounts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "CustomerId",
                keyValue: 1,
                column: "IdentityUserId",
                value: "11111111-1111-1111-1111-111111111111");

            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "CustomerId",
                keyValue: 2,
                column: "IdentityUserId",
                value: "22222222-2222-2222-2222-222222222222");

            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "CustomerId",
                keyValue: 3,
                column: "IdentityUserId",
                value: "33333333-3333-3333-3333-333333333333");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "CustomerId",
                keyValue: 1,
                column: "IdentityUserId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "CustomerId",
                keyValue: 2,
                column: "IdentityUserId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "CustomerId",
                keyValue: 3,
                column: "IdentityUserId",
                value: null);
        }
    }
}
