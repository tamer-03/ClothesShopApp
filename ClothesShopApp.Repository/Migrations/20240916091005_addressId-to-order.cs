using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClothesShopApp.Repository.Migrations
{
    /// <inheritdoc />
    public partial class addressIdtoorder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "addressId",
                table: "orders",
                type: "int",
                nullable: true,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_orders_addressId",
                table: "orders",
                column: "addressId");

            migrationBuilder.AddForeignKey(
                name: "FK_orders_addresses_addressId",
                table: "orders",
                column: "addressId",
                principalTable: "addresses",
                principalColumn: "addressId",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_orders_addresses_addressId",
                table: "orders");

            migrationBuilder.DropIndex(
                name: "IX_orders_addressId",
                table: "orders");

            migrationBuilder.DropColumn(
                name: "addressId",
                table: "orders");
        }
    }
}
