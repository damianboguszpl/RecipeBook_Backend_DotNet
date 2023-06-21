namespace RecipeBook_Backend_DotNet.Services.RecipeServices
{
    public class RecipeService : IRecipeService
    {
        private static List<Recipe> recipes = new List<Recipe>
        {
            new Recipe
            {   Id = 1,
                Name = "first recipe",
                Username = "",
                RecipeCategoryId = 1,
                RecipeDescription = "",
                PrepareTime = "",
                CookTime = "",
                Rating = 4.9,
                PublishingStatus = "",
                Visibility = ""
            },
            new Recipe
            {   Id = 2,
                Name = "second recipe",
                Username = "",
                RecipeCategoryId = 1,
                RecipeDescription = "",
                PrepareTime = "",
                CookTime = "",
                Rating = 2.3,
                PublishingStatus = "",
                Visibility = ""
            }
        };

        public List<Recipe>? GetAllRecipes()
        {
            if (recipes is null)
            {
                return null;
            }
            return recipes;
        }

        public Recipe? GetRecipe(int id)
        {
            var recipe = recipes.Find(x => x.Id == id);
            if (recipe is null)
            {
                return null;
            }
            return recipe;
        }

        public Recipe? AddRecipe(Recipe recipe)
        {
            recipes.Add(recipe);
            var frecipe = recipes.Find(x => x.Id == recipe.Id);
            if (frecipe is null)
            {
                return null;
            }
            return frecipe;
        }

        public Recipe? UpdateRecipe(int id, Recipe request)
        {
            var recipe = recipes.Find(x => x.Id == id);
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
            }
            return recipe;
        }

        public Recipe? DeleteRecipe(int id)
        {
            var recipe = recipes.Find(x => x.Id == id);
            if (recipe is null)
            {
                //return NotFound("This recipe doesn't exist.");
                return null;
            }
            else
            {
                recipes.Remove(recipe);
            }
            //return Ok(recipe);
            return recipe;
        }
    }
}
