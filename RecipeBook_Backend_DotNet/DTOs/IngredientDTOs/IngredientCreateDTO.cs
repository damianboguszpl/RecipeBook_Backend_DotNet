namespace RecipeBook_Backend_DotNet.DTOs.IngredientDTOs
{
    public record struct IngredientCreateDTO(
        string Name,
        int RecipeId
        );
}
