using RecipeBook_Backend_DotNet.DTOs.RecipeDTOs;

namespace RecipeBook_Backend_DotNet.DTOs.CategoryDTOs
{
    public record struct CategoryPackedDTO(
        int Id,
        string Name,
        List<RecipeMinimalDTO> Recipes
        );
}
