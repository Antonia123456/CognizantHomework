using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodShareNET.Repository.Migrations
{
    /// <inheritdoc />
    public partial class ProductImageURL : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                column: "Image",
                value: "https://scitechdaily.com/images/Potato-Sunlight-777x518.jpg");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                column: "Image",
                value: "https://www.tastingtable.com/img/gallery/15-ingredients-that-will-seriously-elevate-your-steak/l-intro-1663169111.jpg");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                column: "Image",
                value: "Potatoes in a basket");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                column: "Image",
                value: "Meat on a plate");
        }
    }
}
