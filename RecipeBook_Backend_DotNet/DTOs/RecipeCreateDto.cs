using System.Net;

namespace RecipeBook_Backend_DotNet.DTOs
{
    public record struct RecipeCreateDto( 
        string Name, 
        string Username,
        string RecipeDescription,
        string PrepareTime,
        string CookTime, 
        float Rating,
        string PublishingStatus,
        string Visibility,
        int UserId,
        int CategoryId
        );
}
