using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecipeBook_Backend_DotNet.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Recipes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RecipeCategoryId = table.Column<int>(type: "int", nullable: false),
                    RecipeDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PrepareTime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CookTime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Rating = table.Column<double>(type: "float", nullable: false),
                    PublishingStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Visibility = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recipes", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Recipes");
        }
    }
}
