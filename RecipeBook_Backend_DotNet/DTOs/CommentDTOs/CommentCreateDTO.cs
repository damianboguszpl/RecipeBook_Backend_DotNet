namespace RecipeBook_Backend_DotNet.DTOs.CommentDTOs
{
    public record struct CommentCreateDTO(
        string Text,
        int UserId,
        int RecipeId
        );
}
