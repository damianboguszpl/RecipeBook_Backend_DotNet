namespace RecipeBook_Backend_DotNet.DTOs
{
    public record struct RecipePackedDTO(
        int Id,
        string Name,
        string RecipeDescription,
        string PrepareTime,
        string CookTime,
        double Rating,
        string PublishingStatus,
        string Visibility,
        UserMinimalDTO User,
        CategoryMinimalDTO Category,
        List<IngredientMinimalDTO> Ingredients,
        List<CommentMinimalDTO> Comments,
        List<LikeMinimalDTO> Likes
        );
}
