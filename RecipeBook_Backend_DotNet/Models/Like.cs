namespace RecipeBook_Backend_DotNet.Models
{
    public class Like
    {
        public int Id { get; set; }

        public User User { get; set; }
        public Recipe Recipe { get; set; }
    }
}
