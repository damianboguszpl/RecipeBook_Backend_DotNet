using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecipeBook_Backend_DotNet.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRatingFieldInRecipeModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rating",
                table: "Recipes");

            migrationBuilder.AddColumn<int>(
                name: "AuthorsRating",
                table: "Recipes",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AuthorsRating",
                table: "Recipes");

            migrationBuilder.AddColumn<float>(
                name: "Rating",
                table: "Recipes",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }
    }
}
