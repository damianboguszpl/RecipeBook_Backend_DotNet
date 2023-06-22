namespace RecipeBook_Backend_DotNet.Models
{
    public class Like
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public required User User { get; set; }
        public int RecipeId { get; set; }
        public required Recipe Recipe { get; set; }
    }
}
