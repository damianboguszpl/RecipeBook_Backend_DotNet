using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecipeBook_Backend_DotNet.Migrations
{
    /// <inheritdoc />
    public partial class UpdateIngredientModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ingredients_Recipes_RecipeId",
                table: "Ingredients");

            migrationBuilder.RenameColumn(
                name: "RecipeId",
                table: "Ingredients",
                newName: "RecipeID");

            migrationBuilder.RenameIndex(
                name: "IX_Ingredients_RecipeId",
                table: "Ingredients",
                newName: "IX_Ingredients_RecipeID");

            migrationBuilder.AddForeignKey(
                name: "FK_Ingredients_Recipes_RecipeID",
                table: "Ingredients",
                column: "RecipeID",
                principalTable: "Recipes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ingredients_Recipes_RecipeID",
                table: "Ingredients");

            migrationBuilder.RenameColumn(
                name: "RecipeID",
                table: "Ingredients",
                newName: "RecipeId");

            migrationBuilder.RenameIndex(
                name: "IX_Ingredients_RecipeID",
                table: "Ingredients",
                newName: "IX_Ingredients_RecipeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ingredients_Recipes_RecipeId",
                table: "Ingredients",
                column: "RecipeId",
                principalTable: "Recipes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
