namespace RecipeBook_Backend_DotNet.DTOs
{
    public record struct LikeMinimalDTO(
        int Id,
        UserMinimalDTO User,
        RecipeMinimalDTO Recipe
        );
}
