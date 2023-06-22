using RecipeBook_Backend_DotNet.DTOs.RecipeDTOs;
using RecipeBook_Backend_DotNet.DTOs.UserDTOs;

namespace RecipeBook_Backend_DotNet.DTOs.CommentDTOs
{
    public record struct CommentPackedDTO(
        int Id,
        string Text,
        UserMinimalDTO User,
        RecipeMinimalDTO Recipe
        );
}
