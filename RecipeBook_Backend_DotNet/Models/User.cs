namespace RecipeBook_Backend_DotNet.Models
{
    public class User
    {
        public int Id { get; set; }
        public string PermissionLevel { get; set; } = String.Empty;
        public string Username { get; set; } = String.Empty;
        public string Password { get; set; } = String.Empty;

        public List<Recipe>? Recipes { get; set; }
        //public List<Comment> Comments { get; set; }
        public List<Like>? Likes { get; set; }
    }
}
