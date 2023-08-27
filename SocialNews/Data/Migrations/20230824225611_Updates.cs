using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialNews.Data.Migrations
{
    /// <inheritdoc />
    public partial class Updates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Votes",
                table: "Posts",
                newName: "Upvotes");

            migrationBuilder.RenameColumn(
                name: "Votes",
                table: "Comments",
                newName: "Upvotes");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Upvotes",
                table: "Posts",
                newName: "Votes");

            migrationBuilder.RenameColumn(
                name: "Upvotes",
                table: "Comments",
                newName: "Votes");
        }
    }
}
