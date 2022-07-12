using Microsoft.EntityFrameworkCore.Migrations;

namespace Juan.Migrations
{
    public partial class AddedBlogAndBlogCategoryUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlogCategories_Blogs_BlogId",
                table: "BlogCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_BlogCategories_Categories_CategoryId",
                table: "BlogCategories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BlogCategories",
                table: "BlogCategories");

            migrationBuilder.RenameTable(
                name: "BlogCategories",
                newName: "BlogBlCategories");

            migrationBuilder.RenameIndex(
                name: "IX_BlogCategories_CategoryId",
                table: "BlogBlCategories",
                newName: "IX_BlogBlCategories_CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_BlogCategories_BlogId",
                table: "BlogBlCategories",
                newName: "IX_BlogBlCategories_BlogId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BlogBlCategories",
                table: "BlogBlCategories",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BlogBlCategories_Blogs_BlogId",
                table: "BlogBlCategories",
                column: "BlogId",
                principalTable: "Blogs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BlogBlCategories_Categories_CategoryId",
                table: "BlogBlCategories",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlogBlCategories_Blogs_BlogId",
                table: "BlogBlCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_BlogBlCategories_Categories_CategoryId",
                table: "BlogBlCategories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BlogBlCategories",
                table: "BlogBlCategories");

            migrationBuilder.RenameTable(
                name: "BlogBlCategories",
                newName: "BlogCategories");

            migrationBuilder.RenameIndex(
                name: "IX_BlogBlCategories_CategoryId",
                table: "BlogCategories",
                newName: "IX_BlogCategories_CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_BlogBlCategories_BlogId",
                table: "BlogCategories",
                newName: "IX_BlogCategories_BlogId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BlogCategories",
                table: "BlogCategories",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BlogCategories_Blogs_BlogId",
                table: "BlogCategories",
                column: "BlogId",
                principalTable: "Blogs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BlogCategories_Categories_CategoryId",
                table: "BlogCategories",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
