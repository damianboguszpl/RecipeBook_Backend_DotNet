﻿using RecipeBook_Backend_DotNet.DTOs.CategoryDTOs;
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

            var recipes = await _context.Recipes
                .Include(c => c.User)
                .Include(c => c.Ingredients)
                .Include(c => c.Category)
                .Include(c => c.Likes)
                .Include(c => c.Comments)
                .ToListAsync();

            if (recipes is null)
                return null;

            foreach (var recipe in recipes)
            {
                UserMinimalDTO userDTO = new() { Id = recipe.User.Id, Username = recipe.User.Username};
                CategoryMinimalDTO categoryDTO = new() { Id = recipe.Category.Id, Name = recipe.Category.Name };
                RecipeMinimalDTO recipeDTO = new() { Id = recipe.Id };

                List<LikeMinimalDTO> likesDTO = recipe.Likes?.Select(like => new LikeMinimalDTO
                {
                    Id = like.Id,
                    UserId = like.UserId,
                    RecipeId = like.RecipeId
                }).ToList() ?? new List<LikeMinimalDTO>();

                List<IngredientMinimalDTO> ingredientsDTO = recipe.Ingredients?.Select(ingredient => new IngredientMinimalDTO
                    {
                        Id = ingredient.Id,
                        Name = ingredient.Name
                    })
                    .ToList() ?? new List<IngredientMinimalDTO>();

                List<CommentMinimalDTO> commentsDTO = recipe.Comments?
                    .Select(comment => new CommentMinimalDTO
                    {
                        Id = comment.Id,
                        Text = comment.Text,
                        User = userDTO
                    })
                    .ToList() ?? new List<CommentMinimalDTO>();

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
            var recipe = await _context.Recipes
                .Include(c => c.User)
                .Include(c => c.Ingredients)
                .Include(c => c.Category)
                .Include(c => c.Likes)
                .Include(c => c.Comments)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (recipe == null)
                return null;

            UserMinimalDTO userDTO = new() { Id = recipe.User.Id, Username = recipe.User.Username };
            CategoryMinimalDTO categoryDTO = new() { Id = recipe.Category.Id, Name = recipe.Category.Name };
            RecipeMinimalDTO recipeMinimalDTO = new() { Id = recipe.Id };

            List<LikeMinimalDTO> likesDTO = recipe.Likes?.Select(like => new LikeMinimalDTO
            {
                Id = like.Id,
                UserId = like.UserId,
                RecipeId = like.RecipeId
            }).ToList() ?? new List<LikeMinimalDTO>();

            List<IngredientMinimalDTO> ingredientsDTO = recipe.Ingredients?.Select(ingredient => new IngredientMinimalDTO
            {
                Id = ingredient.Id,
                Name = ingredient.Name
            })
            .ToList() ?? new List<IngredientMinimalDTO>();

            List<CommentMinimalDTO> commentsDTO = recipe.Comments?.Select(comment => new CommentMinimalDTO
            {
                Id = comment.Id,
                Text = comment.Text,
                User = userDTO
            })
            .ToList() ?? new List<CommentMinimalDTO>();

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
                return null;

            var user = await _context.Users.FindAsync(request.UserId);

            if (user is null)
                return null;

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

            var recipeDTO = new RecipeMinimalDTO { Id = newRecipe.Id };
            return recipeDTO;
        }

        public async Task<RecipeMinimalDTO?> UpdateRecipe(int id, RecipeUpdateDTO request) // TODO: implement DTOs
        {
            var recipe = await _context.Recipes.FindAsync(id);
            if (recipe is null)
                return null;
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

                var recipeDTO = new RecipeMinimalDTO { Id = recipe.Id };
                return recipeDTO;
            }
        }

        public async Task<RecipeMinimalDTO?> DeleteRecipe(int id)
        {
            var recipe = await _context.Recipes.FindAsync(id);
            if (recipe is null)
                return null;
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
