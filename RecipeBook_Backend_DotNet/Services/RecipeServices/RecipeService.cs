using RecipeBook_Backend_DotNet.DTOs.CategoryDTOs;
using RecipeBook_Backend_DotNet.DTOs.CommentDTOs;
using RecipeBook_Backend_DotNet.DTOs.IngredientDTOs;
using RecipeBook_Backend_DotNet.DTOs.LikeDTOs;
using RecipeBook_Backend_DotNet.DTOs.RecipeDTOs;
using RecipeBook_Backend_DotNet.DTOs.UserDTOs;

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
                        User = userDTO
                    })
                    .ToList();

                recipesDTO.Add(new RecipePackedDTO
                {
                    Id = recipe.Id,
                    Name = recipe.Name,
                    RecipeDescription = recipe.RecipeDescription,
                    PrepareTime = recipe.PrepareTime,
                    CookTime = recipe.CookTime,
                    AuthorsRating = recipe.AuthorsRating,
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
                    User = userDTO
                })
                .ToList();

            var recipeDTO = new RecipePackedDTO
            {
                Id = recipe.Id,
                Name = recipe.Name,
                RecipeDescription = recipe.RecipeDescription,
                PrepareTime = recipe.PrepareTime,
                CookTime = recipe.CookTime,
                AuthorsRating = recipe.AuthorsRating,
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

            var user = await _context.Users.FindAsync(request.UserId);
            if (user is null)
            {
                return null;
            }

            var newRecipe = new Recipe
            {
                Name = request.Name,
                Username = request.Username,
                RecipeDescription = request.RecipeDescription,
                PrepareTime = request.PrepareTime,
                CookTime = request.CookTime,
                AuthorsRating = request.AuthorsRating,
                PublishingStatus = request.PublishingStatus,
                Visibility = request.Visibility,
                Category = category,
                User = user
            };

            _context.Recipes.Add(newRecipe);
            await _context.SaveChangesAsync();

            var fRecipe = await _context.Recipes.FindAsync(newRecipe.Id);
            if (fRecipe is null)
                return null;

            var recipeDTO = new RecipeMinimalDTO { Id = fRecipe.Id };
            return recipeDTO;
        }

        public async Task<RecipeMinimalDTO?> UpdateRecipe(int id, RecipeUpdateDTO request) // TODO: implement DTOs
        {
            var recipe = await _context.Recipes.FindAsync(id);
            if (recipe is null)
            {
                return null;
            }
            else
            {
                recipe.Name = request.Name;
                recipe.CategoryId = request.CategoryId;
                recipe.RecipeDescription = request.RecipeDescription;
                recipe.PrepareTime = request.PrepareTime;
                recipe.CookTime = request.CookTime;
                recipe.AuthorsRating = request.AuthorsRating;
                recipe.PublishingStatus = request.PublishingStatus;
                recipe.Visibility = request.Visibility;

                await _context.SaveChangesAsync();
            }

            var recipeDTO = new RecipeMinimalDTO { Id = recipe.Id };
            return recipeDTO;
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

                // Remove Comments

                // Remove Ingredients

                // Remove Likes


                await _context.SaveChangesAsync();
            }

            var recipeDTO = new RecipeMinimalDTO { Id = recipe.Id };
            return recipeDTO;
        }
    }
}
