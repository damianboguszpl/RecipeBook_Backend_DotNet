namespace RecipeBook_Backend_DotNet.Services.RecipeServices
{
    public interface IRecipeService
    {
        Task<List<Recipe>?> GetAllRecipes();
        Task<Recipe?> GetRecipe(int id);
        Task<Recipe?> AddRecipe(Recipe recipe);
        Task<Recipe?> UpdateRecipe(int id, Recipe request);
        Task<Recipe?> DeleteRecipe(int id);
    }
}