using Microsoft.EntityFrameworkCore.Migrations;

namespace Juan.Migrations
{
    public partial class AddedComentRepliesUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ComentReplies_Coments_ComentId",
                table: "ComentReplies");

            migrationBuilder.AlterColumn<int>(
                name: "ComentId",
                table: "ComentReplies",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_ComentReplies_Coments_ComentId",
                table: "ComentReplies",
                column: "ComentId",
                principalTable: "Coments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ComentReplies_Coments_ComentId",
                table: "ComentReplies");

            migrationBuilder.AlterColumn<int>(
                name: "ComentId",
                table: "ComentReplies",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ComentReplies_Coments_ComentId",
                table: "ComentReplies",
                column: "ComentId",
                principalTable: "Coments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
