namespace RecipeBook_Backend_DotNet.Services.RecipeServices
{
    public class RecipeService : IRecipeService
    {
        private readonly DataContext _context;

        public RecipeService(DataContext context)
        {
            _context = context;
        }

        public async Task<List<Recipe>?> GetAllRecipes()
        {
            var recipes = await _context.Recipes.ToListAsync();
            if (recipes is null)
            {
                return null;
            }
            return recipes;
        }

        public async Task<Recipe?> GetRecipe(int id)
        {
            var recipe = await _context.Recipes.FindAsync(id);
            if (recipe is null)
            {
                return null;
            }
            return recipe;
        }

        public async Task<Recipe?> AddRecipe(Recipe recipe)
        {
            //recipes.Add(recipe);
            _context.Recipes.Add(recipe);
            await _context.SaveChangesAsync();

            var frecipe = await _context.Recipes.FindAsync(recipe.Id);
            if (frecipe is null)
            {
                return null;
            }
            return frecipe;
        }

        public async Task<Recipe?> UpdateRecipe(int id, Recipe request)
        {
            var recipe = await _context.Recipes.FindAsync(id);
            if (recipe is null)
            {
                return null;
            }
            else
            {
                recipe.Name = request.Name;
                recipe.Username = request.Username;
                recipe.RecipeCategoryId = request.RecipeCategoryId;
                recipe.RecipeDescription = request.RecipeDescription;
                recipe.PrepareTime = request.PrepareTime;
                recipe.CookTime = request.CookTime;
                recipe.Rating = request.Rating;
                recipe.PublishingStatus = request.PublishingStatus;
                recipe.Visibility = request.Visibility;

                await _context.SaveChangesAsync();
            }
            return recipe;
        }

        public async Task<Recipe?> DeleteRecipe(int id)
        {
            var recipe = await _context.Recipes.FindAsync(id);
            if (recipe is null)
            {
                //return NotFound("This recipe doesn't exist.");
                return null;
            }
            else
            {
                //recipes.Remove(recipe);
                _context.Recipes.Remove(recipe);
                await _context.SaveChangesAsync();
            }
            //return Ok(recipe);
            return recipe;
        }
    }
}
