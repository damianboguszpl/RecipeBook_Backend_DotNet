using Azure.Core;
using RecipeBook_Backend_DotNet.Models;

namespace RecipeBook_Backend_DotNet.DTOs.RecipeDTOs
{
    public record struct RecipeUpdateDTO(
        string Name,
        int CategoryId,
        string RecipeDescription,
        string PrepareTime,
        string CookTime,
        int AuthorsRating,
        string PublishingStatus,
        string Visibility
        );
}
