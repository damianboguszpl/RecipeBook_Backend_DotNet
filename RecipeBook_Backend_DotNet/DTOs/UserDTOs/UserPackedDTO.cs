using RecipeBook_Backend_DotNet.DTOs.LikeDTOs;
using RecipeBook_Backend_DotNet.DTOs.RecipeDTOs;
using RecipeBook_Backend_DotNet.Migrations;

namespace RecipeBook_Backend_DotNet.DTOs.UserDTOs
{
    public record struct UserPackedDTO(
        int Id,
        string Username,
        string PermissionLevel,
        List<RecipeMinimalDTO> Recipes,
        List<LikeMinimalDTO> Likes
        );
}
