using RecipeBook_Backend_DotNet.DTOs.IngredientDTOs;
using RecipeBook_Backend_DotNet.DTOs.RecipeDTOs;
using System.Security.Claims;

namespace RecipeBook_Backend_DotNet.Services.IngredientServices
{
    public class IngredientService : IIngredientService
    {
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public IngredientService(DataContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IngredientMinimalDTO?> AddIngredient(IngredientCreateDTO request)
        {
            var recipe = await _context.Recipes.FindAsync(request.RecipeId);

            if (recipe is null)
                return null;
            else
            {
                if (_httpContextAccessor.HttpContext != null)
                {
                    var currentUserRole = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Role);
                    if (currentUserRole != "admin")
                    {
                        var currentUserId = _httpContextAccessor.HttpContext.User.FindFirstValue("Id");
                        if (currentUserId != recipe.UserId.ToString())
                        {
                            return null; // Forbidden: user has no rights to add this ingredient because it's not related to his recipe
                        }
                    }
                }
                var newIngredient = new Ingredient
                {
                    Name = request.Name,
                    Recipe = recipe
                };

                _context.Ingredients.Add(newIngredient);
                await _context.SaveChangesAsync();

                var ingredientDTO = new IngredientMinimalDTO { Id = newIngredient.Id, Name = newIngredient.Name };
                return ingredientDTO;
            }
        }

        public async Task<IngredientMinimalDTO?> DeleteIngredient(int id)
        {
            var ingredient = await _context.Ingredients
                .Include(c => c.Recipe)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (ingredient is null)
                return null;
            else
            {
                if (_httpContextAccessor.HttpContext != null)
                {
                    var currentUserRole = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Role);
                    if (currentUserRole != "admin")
                    {
                        var currentUserId = _httpContextAccessor.HttpContext.User.FindFirstValue("Id");
                        if (currentUserId != ingredient.Recipe.UserId.ToString())
                        {
                            return null; // Forbidden: user has no rights to delete this ingredient because it's not related to his recipe
                        }
                    }
                }

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
            var recipe = await _context.Recipes
                .Include(c => c.Ingredients)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (recipe is null)
                return null;

            RecipeMinimalDTO recipeDTO = new() { Id = id };

            List<IngredientPackedDTO> ingredientsDTO = recipe.Ingredients?.Select(ingredient => new IngredientPackedDTO
            {
                Id = ingredient.Id,
                Name = ingredient.Name,
                Recipe = recipeDTO
            }).ToList() ?? new List<IngredientPackedDTO>();

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
