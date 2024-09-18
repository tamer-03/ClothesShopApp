using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClothesShopApp.Repository.Migrations
{
    /// <inheritdoc />
    public partial class orderitemaddress : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "stock",
                table: "ProductVariant",
                newName: "Stock");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "orderItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "addressHeader",
                table: "addresses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "orderItems");

            migrationBuilder.DropColumn(
                name: "addressHeader",
                table: "addresses");

            migrationBuilder.RenameColumn(
                name: "Stock",
                table: "ProductVariant",
                newName: "stock");
        }
    }
}
