using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SwizomDbContext.Migrations
{
    /// <inheritdoc />
    public partial class AddedFKtoMenuItemtable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RestaurantID",
                table: "MenuItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_MenuItems_RestaurantID",
                table: "MenuItems",
                column: "RestaurantID");

            migrationBuilder.AddForeignKey(
                name: "FK_MenuItems_Restaurants_RestaurantID",
                table: "MenuItems",
                column: "RestaurantID",
                principalTable: "Restaurants",
                principalColumn: "RestaurantID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MenuItems_Restaurants_RestaurantID",
                table: "MenuItems");

            migrationBuilder.DropIndex(
                name: "IX_MenuItems_RestaurantID",
                table: "MenuItems");

            migrationBuilder.DropColumn(
                name: "RestaurantID",
                table: "MenuItems");
        }
    }
}
