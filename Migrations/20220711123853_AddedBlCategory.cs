using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Juan.Migrations
{
    public partial class AddedBlCategory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BlCategoryId",
                table: "BlogBlCategories",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "BlCategories",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 255, nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    IsUpdated = table.Column<bool>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    DeletedAt = table.Column<DateTime>(nullable: true),
                    UpdatedAt = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlCategories", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BlogBlCategories_BlCategoryId",
                table: "BlogBlCategories",
                column: "BlCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_BlogBlCategories_BlCategories_BlCategoryId",
                table: "BlogBlCategories",
                column: "BlCategoryId",
                principalTable: "BlCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlogBlCategories_BlCategories_BlCategoryId",
                table: "BlogBlCategories");

            migrationBuilder.DropTable(
                name: "BlCategories");

            migrationBuilder.DropIndex(
                name: "IX_BlogBlCategories_BlCategoryId",
                table: "BlogBlCategories");

            migrationBuilder.DropColumn(
                name: "BlCategoryId",
                table: "BlogBlCategories");
        }
    }
}
