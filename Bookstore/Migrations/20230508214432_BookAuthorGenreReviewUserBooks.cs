using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bookstore.Migrations
{
    public partial class BookAuthorGenreReviewUserBooks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookGenre_Book_BookId",
                table: "BookGenre");

            migrationBuilder.DropForeignKey(
                name: "FK_BookGenre_Genre_GenreId",
                table: "BookGenre");

            migrationBuilder.AddForeignKey(
                name: "FK_BookGenre_Book_GenreId",
                table: "BookGenre",
                column: "GenreId",
                principalTable: "Book",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BookGenre_Genre_BookId",
                table: "BookGenre",
                column: "BookId",
                principalTable: "Genre",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookGenre_Book_GenreId",
                table: "BookGenre");

            migrationBuilder.DropForeignKey(
                name: "FK_BookGenre_Genre_BookId",
                table: "BookGenre");

            migrationBuilder.AddForeignKey(
                name: "FK_BookGenre_Book_BookId",
                table: "BookGenre",
                column: "BookId",
                principalTable: "Book",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BookGenre_Genre_GenreId",
                table: "BookGenre",
                column: "GenreId",
                principalTable: "Genre",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
