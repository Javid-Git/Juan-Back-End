using Microsoft.EntityFrameworkCore.Migrations;

namespace Juan.Migrations
{
    public partial class ModelsAndEtcUpdate2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductSizes_Colors_ColorId",
                table: "ProductSizes");

            migrationBuilder.DropIndex(
                name: "IX_ProductSizes_ColorId",
                table: "ProductSizes");

            migrationBuilder.DropColumn(
                name: "ColorId",
                table: "ProductSizes");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ColorId",
                table: "ProductSizes",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductSizes_ColorId",
                table: "ProductSizes",
                column: "ColorId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductSizes_Colors_ColorId",
                table: "ProductSizes",
                column: "ColorId",
                principalTable: "Colors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
