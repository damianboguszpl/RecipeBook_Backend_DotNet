using RecipeBook_Backend_DotNet.DTOs.UserDTOs;

namespace RecipeBook_Backend_DotNet.DTOs.CommentDTOs
{
    public record struct CommentMinimalDTO(
        int Id,
        string Text,
        UserMinimalDTO User
        );
}
