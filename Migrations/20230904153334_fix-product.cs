using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SuperMarketSystem.Migrations
{
    /// <inheritdoc />
    public partial class fixproduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShoppingCartId",
                table: "Products");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ShoppingCartId",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
