using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialNews.Data.Migrations
{
    /// <inheritdoc />
    public partial class Updates2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Posts_PostID",
                table: "Comments");

            migrationBuilder.AlterColumn<int>(
                name: "PostID",
                table: "Comments",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateTable(
                name: "ApplicationUserComment",
                columns: table => new
                {
                    UpvotedCommentsID = table.Column<int>(type: "int", nullable: false),
                    UsersUpvotingId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUserComment", x => new { x.UpvotedCommentsID, x.UsersUpvotingId });
                    table.ForeignKey(
                        name: "FK_ApplicationUserComment_AspNetUsers_UsersUpvotingId",
                        column: x => x.UsersUpvotingId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicationUserComment_Comments_UpvotedCommentsID",
                        column: x => x.UpvotedCommentsID,
                        principalTable: "Comments",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationUserPost",
                columns: table => new
                {
                    UpvotedPostsID = table.Column<int>(type: "int", nullable: false),
                    UsersUpvotingId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUserPost", x => new { x.UpvotedPostsID, x.UsersUpvotingId });
                    table.ForeignKey(
                        name: "FK_ApplicationUserPost_AspNetUsers_UsersUpvotingId",
                        column: x => x.UsersUpvotingId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicationUserPost_Posts_UpvotedPostsID",
                        column: x => x.UpvotedPostsID,
                        principalTable: "Posts",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationUserPost1",
                columns: table => new
                {
                    SavedPostsID = table.Column<int>(type: "int", nullable: false),
                    UsersSavingId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUserPost1", x => new { x.SavedPostsID, x.UsersSavingId });
                    table.ForeignKey(
                        name: "FK_ApplicationUserPost1_AspNetUsers_UsersSavingId",
                        column: x => x.UsersSavingId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicationUserPost1_Posts_SavedPostsID",
                        column: x => x.SavedPostsID,
                        principalTable: "Posts",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserComment_UsersUpvotingId",
                table: "ApplicationUserComment",
                column: "UsersUpvotingId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserPost_UsersUpvotingId",
                table: "ApplicationUserPost",
                column: "UsersUpvotingId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserPost1_UsersSavingId",
                table: "ApplicationUserPost1",
                column: "UsersSavingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Posts_PostID",
                table: "Comments",
                column: "PostID",
                principalTable: "Posts",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Posts_PostID",
                table: "Comments");

            migrationBuilder.DropTable(
                name: "ApplicationUserComment");

            migrationBuilder.DropTable(
                name: "ApplicationUserPost");

            migrationBuilder.DropTable(
                name: "ApplicationUserPost1");

            migrationBuilder.AlterColumn<int>(
                name: "PostID",
                table: "Comments",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Posts_PostID",
                table: "Comments",
                column: "PostID",
                principalTable: "Posts",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
