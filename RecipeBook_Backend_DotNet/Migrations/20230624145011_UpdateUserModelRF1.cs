using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecipeBook_Backend_DotNet.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserModelRF1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_RefreshTokenId",
                table: "Users");

            migrationBuilder.CreateIndex(
                name: "IX_Users_RefreshTokenId",
                table: "Users",
                column: "RefreshTokenId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_RefreshTokenId",
                table: "Users");

            migrationBuilder.CreateIndex(
                name: "IX_Users_RefreshTokenId",
                table: "Users",
                column: "RefreshTokenId");
        }
    }
}
