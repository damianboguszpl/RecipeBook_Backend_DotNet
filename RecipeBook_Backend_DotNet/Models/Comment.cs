namespace RecipeBook_Backend_DotNet.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Text { get; set; } = String.Empty;
        public string Username { get; set; } = String.Empty;

        public User User { get; set; }
        public Recipe Recipe { get; set; }
    }
}
