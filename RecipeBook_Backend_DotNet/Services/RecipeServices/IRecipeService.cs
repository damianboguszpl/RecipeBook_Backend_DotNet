using RecipeBook_Backend_DotNet.DTOs;

namespace RecipeBook_Backend_DotNet.Services.RecipeServices
{
    public interface IRecipeService
    {
        Task<List<RecipePackedDTO>?> GetAllRecipes();
        Task<RecipePackedDTO?> GetRecipe(int id);
        Task<RecipeMinimalDTO?> AddRecipe(RecipeCreateDto request);
        Task<Recipe?> UpdateRecipe(int id, Recipe request);
        Task<RecipeMinimalDTO?> DeleteRecipe(int id);
    }
}