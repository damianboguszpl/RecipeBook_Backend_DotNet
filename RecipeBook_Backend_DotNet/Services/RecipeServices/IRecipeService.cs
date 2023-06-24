using RecipeBook_Backend_DotNet.DTOs.IngredientDTOs;
using RecipeBook_Backend_DotNet.DTOs.RecipeDTOs;

namespace RecipeBook_Backend_DotNet.Services.RecipeServices
{
    public interface IRecipeService
    {
        Task<List<RecipePackedDTO>?> GetAllRecipes();
        Task<List<RecipePackedDTO>?> GetAllPublicRecipes();
        Task<RecipePackedDTO?> GetRecipe(int id);
        Task<List<RecipePackedDTO>?> GetAllRecipesByUser(int id);
        Task<RecipeMinimalDTO?> AddRecipe(RecipeCreateDto request);
        Task<RecipeMinimalDTO?> UpdateRecipe(int id, RecipeUpdateDTO request);
        Task<RecipeMinimalDTO?> DeleteRecipe(int id);
    }
}