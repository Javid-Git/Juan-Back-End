using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Juan.Migrations
{
    public partial class AddedComentReplies : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MainComentId",
                table: "Coments");

            migrationBuilder.CreateTable(
                name: "ComentReplies",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Text = table.Column<string>(maxLength: 500, nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    BlogId = table.Column<int>(nullable: true),
                    ProductId = table.Column<int>(nullable: true),
                    UserId = table.Column<string>(nullable: true),
                    Rating = table.Column<int>(nullable: false),
                    ComentId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComentReplies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ComentReplies_Blogs_BlogId",
                        column: x => x.BlogId,
                        principalTable: "Blogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ComentReplies_Coments_ComentId",
                        column: x => x.ComentId,
                        principalTable: "Coments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ComentReplies_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ComentReplies_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ComentReplies_BlogId",
                table: "ComentReplies",
                column: "BlogId");

            migrationBuilder.CreateIndex(
                name: "IX_ComentReplies_ComentId",
                table: "ComentReplies",
                column: "ComentId");

            migrationBuilder.CreateIndex(
                name: "IX_ComentReplies_ProductId",
                table: "ComentReplies",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ComentReplies_UserId",
                table: "ComentReplies",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ComentReplies");

            migrationBuilder.AddColumn<int>(
                name: "MainComentId",
                table: "Coments",
                type: "int",
                nullable: true);
        }
    }
}
