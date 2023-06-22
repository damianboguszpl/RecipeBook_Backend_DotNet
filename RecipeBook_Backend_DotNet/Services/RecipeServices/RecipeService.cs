using Azure.Core;
using RecipeBook_Backend_DotNet.DTOs;
using RecipeBook_Backend_DotNet.Models;
using System.Xml.Linq;

namespace RecipeBook_Backend_DotNet.Services.RecipeServices
{
    public class RecipeService : IRecipeService
    {
        private readonly DataContext _context;

        public RecipeService(DataContext context)
        {
            _context = context;
        }

        public async Task<List<RecipePackedDTO>?> GetAllRecipes()
        {
            List<RecipePackedDTO> recipesDTO = new();

            var recipes = await _context.Recipes.ToListAsync();

            if (recipes is null)
                return null;

            foreach (var recipe in recipes)
            {
                var category = await _context.Categories.FindAsync(recipe.CategoryId);
                if (category is null)
                    return null;

                recipe.Likes = await _context.Likes
                    .Where(l => l.Recipe.Id == recipe.Id)
                    .ToListAsync();
                recipe.Comments = await _context.Comments
                    .Where(l => l.Recipe.Id == recipe.Id)
                    .ToListAsync();

                var user = await _context.Users.FindAsync(recipe.UserId);
                if (user is null)
                    return null;

                UserMinimalDTO userDTO = new() { Id = user.Id, Username = user.Username};
                CategoryMinimalDTO categoryDTO = new() { Id = category.Id, Name = category.Name };
                RecipeMinimalDTO recipeDTO = new() { Id = recipe.Id };

                List<LikeMinimalDTO> likesDTO = recipe.Likes.Select(like => new LikeMinimalDTO
                {
                    Id = like.Id,
                    Recipe = recipeDTO,
                    User = userDTO
                }).ToList();

                List<IngredientMinimalDTO> ingredientsDTO = (
                    await _context.Ingredients
                    .Where(l => l.Recipe.Id == recipe.Id)
                    .ToListAsync()
                    )
                    ?.Select(ingredient => new IngredientMinimalDTO
                    {
                        Id = ingredient.Id,
                        Name = ingredient.Name
                    })
                    .ToList() ?? new List<IngredientMinimalDTO>();

                List<CommentMinimalDTO> commentsDTO = recipe.Comments
                    .Select(comment => new CommentMinimalDTO
                    {
                        Id = comment.Id,
                        Text = comment.Text,
                        Username = userDTO.Username
                    })
                    .ToList();

                recipesDTO.Add(new RecipePackedDTO
                {
                    Id = recipe.Id,
                    Name = recipe.Name,
                    RecipeDescription = recipe.RecipeDescription,
                    PrepareTime = recipe.PrepareTime,
                    CookTime = recipe.CookTime,
                    Rating = recipe.Rating,
                    PublishingStatus = recipe.PublishingStatus,
                    Visibility = recipe.Visibility,
                    Category = categoryDTO,
                    User = userDTO,
                    Comments = commentsDTO,
                    Likes = likesDTO,
                    Ingredients = ingredientsDTO,
                });
            }
            return recipesDTO;
        }

        public async Task<RecipePackedDTO?> GetRecipe(int id)
        {
            var recipe = await _context.Recipes.FindAsync(id);
            if (recipe is null)
                return null;

            var category = await _context.Categories.FindAsync(recipe.CategoryId);
            if (category is null)
                return null;

            recipe.Likes = await _context.Likes
                .Where(l => l.Recipe.Id == recipe.Id)
                .ToListAsync();
            recipe.Comments = await _context.Comments
                .Where(l => l.Recipe.Id == recipe.Id)
                .ToListAsync();

            var user = await _context.Users.FindAsync(recipe.UserId);
            if (user is null)
                return null;

            UserMinimalDTO userDTO = new() { Id = user.Id, Username = user.Username };
            CategoryMinimalDTO categoryDTO = new() { Id = category.Id, Name = category.Name };
            RecipeMinimalDTO recipeMinimalDTO = new() { Id = recipe.Id };

            List<LikeMinimalDTO> likesDTO = recipe.Likes.Select(like => new LikeMinimalDTO
            {
                Id = like.Id,
                Recipe = recipeMinimalDTO,
                User = userDTO
            }).ToList();

            List<IngredientMinimalDTO> ingredientsDTO = (
                await _context.Ingredients
                .Where(l => l.Recipe.Id == recipe.Id)
                .ToListAsync()
                )
                ?.Select(ingredient => new IngredientMinimalDTO
                {
                    Id = ingredient.Id,
                    Name = ingredient.Name
                })
                .ToList() ?? new List<IngredientMinimalDTO>();

            List<CommentMinimalDTO> commentsDTO = recipe.Comments
                .Select(comment => new CommentMinimalDTO
                {
                    Id = comment.Id,
                    Text = comment.Text,
                    Username = userDTO.Username
                })
                .ToList();

            var recipeDTO = new RecipePackedDTO
            {
                Id = recipe.Id,
                Name = recipe.Name,
                RecipeDescription = recipe.RecipeDescription,
                PrepareTime = recipe.PrepareTime,
                CookTime = recipe.CookTime,
                Rating = recipe.Rating,
                PublishingStatus = recipe.PublishingStatus,
                Visibility = recipe.Visibility,
                Category = categoryDTO,
                User = userDTO,
                Comments = commentsDTO,
                Likes = likesDTO,
                Ingredients = ingredientsDTO,
            };

            return recipeDTO;
        }

        public async Task<RecipeMinimalDTO?> AddRecipe(RecipeCreateDto request)
        {
            var category = await _context.Categories.FindAsync(request.CategoryId);
            if (category is null)
            {
                return null;
            }
            //newRecipe.Category = category;

            var user = await _context.Users.FindAsync(request.UserId);
            if (user is null)
            {
                return null;
            }
            //newRecipe.User = user;

            var newRecipe = new Recipe
            {
                Name = request.Name,
                Username = request.Username,
                RecipeDescription = request.RecipeDescription,
                PrepareTime = request.PrepareTime,
                CookTime = request.CookTime,
                Rating = request.Rating,
                PublishingStatus = request.PublishingStatus,
                Visibility = request.Visibility,
                Category = category,
                User = user
            };

            //recipes.Add(recipe);

            _context.Recipes.Add(newRecipe);
            await _context.SaveChangesAsync();

            var frecipe = await _context.Recipes.FindAsync(newRecipe.Id);
            if (frecipe is null)
            {
                return null;
            }
            var recipeDTO = new RecipeMinimalDTO { Id = frecipe.Id };
            return recipeDTO;
        }

        public async Task<Recipe?> UpdateRecipe(int id, Recipe request) // TODO: implement DTOs
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

        public async Task<RecipeMinimalDTO?> DeleteRecipe(int id)
        {
            var recipe = await _context.Recipes.FindAsync(id);
            if (recipe is null)
            {
                return null;
            }
            else
            {
                _context.Recipes.Remove(recipe);
                await _context.SaveChangesAsync();
            }

            var recipeDTO = new RecipeMinimalDTO { Id = recipe.Id };
            return recipeDTO;
        }
    }
}
