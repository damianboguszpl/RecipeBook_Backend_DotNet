namespace RecipeBook_Backend_DotNet.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = String.Empty;
        public string PicUrl { get; set; } = String.Empty;

        public List<Recipe> Recipes { get; set; }
    }
}
