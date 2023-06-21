namespace RecipeBook_Backend_DotNet.Services.RecipeServices
{
    public interface IRecipeService
    {
        List<Recipe>? GetAllRecipes();
        Recipe? GetRecipe(int id);
        Recipe? AddRecipe(Recipe recipe);
        Recipe? UpdateRecipe(int id, Recipe request);
        Recipe? DeleteRecipe(int id);
    }
}