using RecipeBook_Backend_DotNet.DTOs.RecipeDTOs;
using RecipeBook_Backend_DotNet.DTOs.UserDTOs;

namespace RecipeBook_Backend_DotNet.DTOs.LikeDTOs
{
    public record struct LikeMinimalDTO(
        int Id,
        UserMinimalDTO User,
        RecipeMinimalDTO Recipe
        );
}
