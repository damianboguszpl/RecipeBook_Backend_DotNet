using System.Net;

namespace RecipeBook_Backend_DotNet.DTOs.RecipeDTOs
{
    public record struct RecipeCreateDto(
        string Name,
        string Username,
        string RecipeDescription,
        string PrepareTime,
        string CookTime,
        int AuthorsRating,
        string PublishingStatus,
        string Visibility,
        int UserId,
        int CategoryId
        );
}
