using RecipeBook_Backend_DotNet.DTOs.IngredientDTOs;

namespace RecipeBook_Backend_DotNet.Services.IngredientServices
{
    public interface IIngredientService
    {
        Task<List<IngredientPackedDTO>?> GetAllIngredients();
        Task<List<IngredientPackedDTO>?> GetAllPublicIngredients();
        Task<IngredientPackedDTO?> GetIngredient(int id);
        Task<List<IngredientPackedDTO>?> GetAllIngredientsByRecipe(int id);
        Task<IngredientMinimalDTO?> AddIngredient(IngredientCreateDTO request);
        Task<IngredientMinimalDTO?> DeleteIngredient(int id);
    }
}
