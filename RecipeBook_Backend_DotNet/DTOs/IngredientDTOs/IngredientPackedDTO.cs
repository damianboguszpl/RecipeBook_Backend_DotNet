using RecipeBook_Backend_DotNet.DTOs.RecipeDTOs;

namespace RecipeBook_Backend_DotNet.DTOs.IngredientDTOs
{
    public record struct IngredientPackedDTO(
        int Id,
        string Name,
        RecipeMinimalDTO Recipe
        );
}
