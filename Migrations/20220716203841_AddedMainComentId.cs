using Microsoft.EntityFrameworkCore.Migrations;

namespace Juan.Migrations
{
    public partial class AddedMainComentId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MainComentId",
                table: "Coments",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MainComentId",
                table: "Coments");
        }
    }
}
