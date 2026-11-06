using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace BooksApi.Server.Migrations
{
    /// <inheritdoc />
    public partial class SecondMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                schema: "BooksSchema",
                table: "Books",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Users",
                schema: "BooksSchema",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Username = table.Column<string>(type: "text", nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserBookRelation",
                schema: "BooksSchema",
                columns: table => new
                {
                    BookId = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserBookRelation", x => new { x.UserId, x.BookId });
                    table.ForeignKey(
                        name: "FK_UserBookRelation_Books_BookId",
                        column: x => x.BookId,
                        principalSchema: "BooksSchema",
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserBookRelation_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "BooksSchema",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Books_UserId",
                schema: "BooksSchema",
                table: "Books",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserBookRelation_BookId",
                schema: "BooksSchema",
                table: "UserBookRelation",
                column: "BookId");

            migrationBuilder.AddForeignKey(
                name: "FK_Books_Users_UserId",
                schema: "BooksSchema",
                table: "Books",
                column: "UserId",
                principalSchema: "BooksSchema",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_Users_UserId",
                schema: "BooksSchema",
                table: "Books");

            migrationBuilder.DropTable(
                name: "UserBookRelation",
                schema: "BooksSchema");

            migrationBuilder.DropTable(
                name: "Users",
                schema: "BooksSchema");

            migrationBuilder.DropIndex(
                name: "IX_Books_UserId",
                schema: "BooksSchema",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "UserId",
                schema: "BooksSchema",
                table: "Books");
        }
    }
}
