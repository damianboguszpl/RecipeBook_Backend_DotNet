namespace RecipeBook_Backend_DotNet.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Text { get; set; } = String.Empty;
        public string Username { get; set; } = String.Empty;
        public int UserId { get; set; }
        public required User User { get; set; }
        public int RecipeId { get; set; }
        public required Recipe Recipe { get; set; }
    }
}
