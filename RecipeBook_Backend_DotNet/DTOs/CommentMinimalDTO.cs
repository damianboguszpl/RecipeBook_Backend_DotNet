namespace RecipeBook_Backend_DotNet.DTOs
{
    public record struct CommentMinimalDTO(
        int Id,
        string Text,
        string Username
        );
}
