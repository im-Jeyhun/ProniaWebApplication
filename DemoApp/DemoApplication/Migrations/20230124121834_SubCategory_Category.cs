using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DemoApplication.Migrations
{
    public partial class SubCategory_Category : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubCategories_Plants_PlantId",
                table: "SubCategories");

            migrationBuilder.DropIndex(
                name: "IX_SubCategories_PlantId",
                table: "SubCategories");

            migrationBuilder.DropColumn(
                name: "PlantId",
                table: "SubCategories");

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "Plants",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Sku",
                table: "Plants",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "SubCategoryId",
                table: "Plants",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Plants_CategoryId",
                table: "Plants",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Plants_SubCategoryId",
                table: "Plants",
                column: "SubCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Plants_Categories_CategoryId",
                table: "Plants",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Plants_SubCategories_SubCategoryId",
                table: "Plants",
                column: "SubCategoryId",
                principalTable: "SubCategories",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Plants_Categories_CategoryId",
                table: "Plants");

            migrationBuilder.DropForeignKey(
                name: "FK_Plants_SubCategories_SubCategoryId",
                table: "Plants");

            migrationBuilder.DropIndex(
                name: "IX_Plants_CategoryId",
                table: "Plants");

            migrationBuilder.DropIndex(
                name: "IX_Plants_SubCategoryId",
                table: "Plants");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Plants");

            migrationBuilder.DropColumn(
                name: "Sku",
                table: "Plants");

            migrationBuilder.DropColumn(
                name: "SubCategoryId",
                table: "Plants");

            migrationBuilder.AddColumn<int>(
                name: "PlantId",
                table: "SubCategories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_SubCategories_PlantId",
                table: "SubCategories",
                column: "PlantId");

            migrationBuilder.AddForeignKey(
                name: "FK_SubCategories_Plants_PlantId",
                table: "SubCategories",
                column: "PlantId",
                principalTable: "Plants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
