using RecipeBook_Backend_DotNet.DTOs.RecipeDTOs;

namespace RecipeBook_Backend_DotNet.Services.RecipeServices
{
    public interface IRecipeService
    {
        Task<List<RecipePackedDTO>?> GetAllRecipes();
        Task<RecipePackedDTO?> GetRecipe(int id);
        Task<RecipeMinimalDTO?> AddRecipe(RecipeCreateDto request);
        Task<RecipeMinimalDTO?> UpdateRecipe(int id, RecipeUpdateDTO request);
        Task<RecipeMinimalDTO?> DeleteRecipe(int id);
    }
}