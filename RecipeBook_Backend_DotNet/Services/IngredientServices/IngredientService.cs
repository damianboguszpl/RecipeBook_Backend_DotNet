﻿using RecipeBook_Backend_DotNet.DTOs.IngredientDTOs;
using RecipeBook_Backend_DotNet.DTOs.RecipeDTOs;

namespace RecipeBook_Backend_DotNet.Services.IngredientServices
{
    public class IngredientService : IIngredientService
    {
        private readonly DataContext _context;

        public IngredientService(DataContext context)
        {
            _context = context;
        }

        public async Task<IngredientMinimalDTO?> AddIngredient(IngredientCreateDTO request)
        {
            var recipe = await _context.Recipes.FindAsync(request.RecipeId);
            if (recipe is null)
                return null;

            var newIngredient = new Ingredient { 
                Name = request.Name, 
                Recipe = recipe
            };

            _context.Ingredients.Add(newIngredient);
            await _context.SaveChangesAsync();

            var fIngredient = await _context.Ingredients.FindAsync(newIngredient.Id);
            if (fIngredient is null)
                return null;

            var ingredientDTO = new IngredientMinimalDTO { Id = fIngredient.Id, Name = fIngredient.Name };
            return ingredientDTO;
        }

        public async Task<IngredientMinimalDTO?> DeleteIngredient(int id)
        {
            var ingredient = await _context.Ingredients.FindAsync(id);
            if (ingredient is null)
                return null;
            else
            {
                _context.Ingredients.Remove(ingredient);
                await _context.SaveChangesAsync();
            }

            var ingredientDTO = new IngredientMinimalDTO { Id = ingredient.Id , Name = ingredient.Name};
            return ingredientDTO;
        }

        public async Task<List<IngredientPackedDTO>?> GetAllIngredients()
        {
            var ingredients = await _context.Ingredients.ToListAsync();
            if (ingredients is null)
                return null;

            List<IngredientPackedDTO> ingredientsDTO = ingredients.Select(ingredient => new IngredientPackedDTO
            {
                Id = ingredient.Id,
                Name = ingredient.Name,
                Recipe = new RecipeMinimalDTO { Id = ingredient.RecipeID }
            }).ToList();

            return ingredientsDTO;
        }

        public async Task<List<IngredientPackedDTO>?> GetAllIngredientsByRecipe(int id)
        {
            var recipe = await _context.Recipes.FindAsync(id);
            if (recipe is null)
                return null;
            RecipeMinimalDTO recipeDTO = new() { Id = id };

            var ingredients = await _context.Ingredients.
                Where(recipe => recipe.RecipeID == id).
                ToListAsync();
            if (ingredients is null)
                return null;

            List<IngredientPackedDTO> ingredientsDTO = ingredients.Select(ingredient => new IngredientPackedDTO
            {
                Id = ingredient.Id,
                Name = ingredient.Name,
                Recipe = recipeDTO
            }).ToList();

            return ingredientsDTO;
        }

        public async Task<IngredientPackedDTO?> GetIngredient(int id)
        {
            var ingredient = await _context.Ingredients.FindAsync(id);
            if (ingredient is null)
                return null;
            return new IngredientPackedDTO {
                Id = ingredient.Id,
                Name = ingredient.Name,
                Recipe = new RecipeMinimalDTO {
                    Id = ingredient.RecipeID
                }
            };
        }
    }
}