namespace RecipeBook_Backend_DotNet.Models
{
    public class Ingredient
    {
        public int Id { get; set; }
        public string Name { get; set; } = String.Empty;

        public int RecipeID { get; set; }
        public required Recipe Recipe { get; set; }
    }
}
