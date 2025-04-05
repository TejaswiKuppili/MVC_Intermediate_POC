using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SwizomDbContext.Migrations
{
    /// <inheritdoc />
    public partial class CustomerEmailcolumnUpdatedinOrdersTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetail_MenuItems_ItemID",
                table: "OrderDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetail_Orders_OrderID",
                table: "OrderDetail");

            migrationBuilder.AddColumn<string>(
                name: "CustomerEmail",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetail_MenuItems_ItemID",
                table: "OrderDetail",
                column: "ItemID",
                principalTable: "MenuItems",
                principalColumn: "ItemID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetail_Orders_OrderID",
                table: "OrderDetail",
                column: "OrderID",
                principalTable: "Orders",
                principalColumn: "OrderID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetail_MenuItems_ItemID",
                table: "OrderDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetail_Orders_OrderID",
                table: "OrderDetail");

            migrationBuilder.DropColumn(
                name: "CustomerEmail",
                table: "Orders");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetail_MenuItems_ItemID",
                table: "OrderDetail",
                column: "ItemID",
                principalTable: "MenuItems",
                principalColumn: "ItemID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetail_Orders_OrderID",
                table: "OrderDetail",
                column: "OrderID",
                principalTable: "Orders",
                principalColumn: "OrderID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
