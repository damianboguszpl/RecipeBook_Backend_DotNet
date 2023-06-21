namespace RecipeBook_Backend_DotNet.Models
{
    public class Ingredient
    {
        public int Id { get; set; }
        public string Name { get; set; } = String.Empty;

        public Recipe Recipe { get; set; }
    }
}
