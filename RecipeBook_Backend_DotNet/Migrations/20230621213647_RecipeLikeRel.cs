using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecipeBook_Backend_DotNet.Migrations
{
    /// <inheritdoc />
    public partial class RecipeLikeRel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RecipeId",
                table: "Like",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Like_RecipeId",
                table: "Like",
                column: "RecipeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Like_Recipes_RecipeId",
                table: "Like",
                column: "RecipeId",
                principalTable: "Recipes",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Like_Recipes_RecipeId",
                table: "Like");

            migrationBuilder.DropIndex(
                name: "IX_Like_RecipeId",
                table: "Like");

            migrationBuilder.DropColumn(
                name: "RecipeId",
                table: "Like");
        }
    }
}
