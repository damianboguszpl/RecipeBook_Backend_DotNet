namespace RecipeBook_Backend_DotNet.DTOs.CommentDTOs
{
    public record struct CommentMinimalDTO(
        int Id,
        string Text,
        string Username
        );
}
