using System.Text.Json.Serialization;

namespace RecipeBook_Backend_DotNet.Models
{
    public class Like
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        [JsonIgnore]
        public required User User { get; set; }
        public int RecipeId { get; set; }
        [JsonIgnore]
        public required Recipe Recipe { get; set; }
    }
}
