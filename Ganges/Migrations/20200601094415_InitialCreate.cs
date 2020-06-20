using Microsoft.EntityFrameworkCore.Migrations;

namespace Ganges.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Seller = table.Column<string>(nullable: true),
                    Price = table.Column<int>(nullable: false),
                    Quantity = table.Column<int>(nullable: false),
                    ImageUrl = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Description", "ImageUrl", "Price", "Quantity", "Seller", "Title" },
                values: new object[] { 1, "Glass", "table.png", 100, 2, "George", "Table" });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Description", "ImageUrl", "Price", "Quantity", "Seller", "Title" },
                values: new object[] { 2, "Wooden", "chair.png", 50, 5, "Kevin", "Chair" });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Description", "ImageUrl", "Price", "Quantity", "Seller", "Title" },
                values: new object[] { 3, "High performance", "computer.png", 800, 1, "James", "Computer" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}
