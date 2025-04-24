using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class FixProductFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_Products_ProductId1",
                schema: "public",
                table: "CartItems");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetails_Products_ProductId1",
                schema: "public",
                table: "OrderDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Discounts_DiscountId",
                schema: "public",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_OrderDetails_ProductId1",
                schema: "public",
                table: "OrderDetails");

            migrationBuilder.DropIndex(
                name: "IX_CartItems_ProductId1",
                schema: "public",
                table: "CartItems");

            migrationBuilder.DropColumn(
                name: "ProductId1",
                schema: "public",
                table: "OrderDetails");

            migrationBuilder.DropColumn(
                name: "ProductId1",
                schema: "public",
                table: "CartItems");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Discounts_DiscountId",
                schema: "public",
                table: "Products",
                column: "DiscountId",
                principalSchema: "public",
                principalTable: "Discounts",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Discounts_DiscountId",
                schema: "public",
                table: "Products");

            migrationBuilder.AddColumn<int>(
                name: "ProductId1",
                schema: "public",
                table: "OrderDetails",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProductId1",
                schema: "public",
                table: "CartItems",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_ProductId1",
                schema: "public",
                table: "OrderDetails",
                column: "ProductId1");

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_ProductId1",
                schema: "public",
                table: "CartItems",
                column: "ProductId1");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_Products_ProductId1",
                schema: "public",
                table: "CartItems",
                column: "ProductId1",
                principalSchema: "public",
                principalTable: "Products",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetails_Products_ProductId1",
                schema: "public",
                table: "OrderDetails",
                column: "ProductId1",
                principalSchema: "public",
                principalTable: "Products",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Discounts_DiscountId",
                schema: "public",
                table: "Products",
                column: "DiscountId",
                principalSchema: "public",
                principalTable: "Discounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
