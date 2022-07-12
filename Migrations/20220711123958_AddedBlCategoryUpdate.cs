using Microsoft.EntityFrameworkCore.Migrations;

namespace Juan.Migrations
{
    public partial class AddedBlCategoryUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlogBlCategories_Categories_CategoryId",
                table: "BlogBlCategories");

            migrationBuilder.DropIndex(
                name: "IX_BlogBlCategories_CategoryId",
                table: "BlogBlCategories");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_BlogBlCategories_CategoryId",
                table: "BlogBlCategories",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_BlogBlCategories_Categories_CategoryId",
                table: "BlogBlCategories",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
