using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClothesShopApp.Repository.Migrations
{
    /// <inheritdoc />
    public partial class addednamelastnameaddress : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "firstName",
                table: "addresses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "lastName",
                table: "addresses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "firstName",
                table: "addresses");

            migrationBuilder.DropColumn(
                name: "lastName",
                table: "addresses");
        }
    }
}
