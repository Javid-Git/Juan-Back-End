using Microsoft.EntityFrameworkCore.Migrations;

namespace Juan.Migrations
{
    public partial class AddedBlCategoryUpdate2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlogBlCategories_BlCategories_BlCategoryId",
                table: "BlogBlCategories");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "BlogBlCategories");

            migrationBuilder.AlterColumn<int>(
                name: "BlCategoryId",
                table: "BlogBlCategories",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_BlogBlCategories_BlCategories_BlCategoryId",
                table: "BlogBlCategories",
                column: "BlCategoryId",
                principalTable: "BlCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlogBlCategories_BlCategories_BlCategoryId",
                table: "BlogBlCategories");

            migrationBuilder.AlterColumn<int>(
                name: "BlCategoryId",
                table: "BlogBlCategories",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "BlogBlCategories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_BlogBlCategories_BlCategories_BlCategoryId",
                table: "BlogBlCategories",
                column: "BlCategoryId",
                principalTable: "BlCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
