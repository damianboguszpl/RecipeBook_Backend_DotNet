using RecipeBook_Backend_DotNet.DTOs.CategoryDTOs;
using RecipeBook_Backend_DotNet.DTOs.IngredientDTOs;
using RecipeBook_Backend_DotNet.DTOs.RecipeDTOs;

namespace RecipeBook_Backend_DotNet.Services.CategoryServices
{
    public class CategoryService : ICategoryService
    {
        private readonly DataContext _context;

        public CategoryService(DataContext context)
        {
            _context = context;
        }

        public async Task<CategoryMinimalDTO?> AddCategory(CategoryCreateDTO request)
        {
            if (request.Name is null || request.PicUrl is null)
                return null;

            var category = await _context.Categories.
                FirstOrDefaultAsync(category => category.Name == request.Name);
            
            if (category == null)
            {
                var newCategory = new Category { 
                    Name = request.Name, 
                    PicUrl = request.PicUrl
                };
                _context.Categories.Add(newCategory);
                await _context.SaveChangesAsync();

                var categoryDTO = new CategoryMinimalDTO { 
                    Id = newCategory.Id, 
                    Name = newCategory.Name
                };
                return categoryDTO;
            }
            else
                return null;
        }

        public async Task<CategoryMinimalDTO?> DeleteCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category is null)
                return null;
            else
            {
                var recipes = await _context.Recipes
                    .Where(recipe => recipe.CategoryId == category.Id).
                    ToListAsync();

                if (recipes.Count < 1)
                {
                    _context.Categories.Remove(category);
                    await _context.SaveChangesAsync();
                }
                else
                    return null; // can't delete category with existing recipes; TODO: implement responseCodes insted of returning null (here & in other similiar cases)
            }

            var categoryDTO = new CategoryMinimalDTO { Id = category.Id, Name = category.Name};
            return categoryDTO;
        }

        public async Task<List<CategoryPackedDTO>?> GetAllCategories()
        {
            var categories = await _context.Categories.ToListAsync();
            if (categories is null)
                return null;

            List<CategoryPackedDTO> categoriesDTO = new();
            foreach (var category in categories)
            {
                var recipesDTO = await _context.Recipes
                    .Where(recipe => recipe.CategoryId == category.Id)
                    .Select(recipe => new RecipeMinimalDTO
                    {
                        Id = recipe.Id
                    })
                    .ToListAsync();

                categoriesDTO.Add(new CategoryPackedDTO
                {
                    Id = category.Id,
                    Name = category.Name,
                    Recipes = recipesDTO
                });
            }
            return categoriesDTO;
        }

        public async Task<CategoryPackedDTO?> GetCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category is null)
                return null;
            var recipes = await _context.Recipes.
                Where(recipe => recipe.CategoryId == category.Id)
                .ToListAsync();

            List<RecipeMinimalDTO> recipesDTO = recipes.Select( recipe => new RecipeMinimalDTO
            {
                Id = recipe.Id
            }).ToList();

            return new CategoryPackedDTO
            {
                Id = category.Id,
                Name = category.Name,
                Recipes = recipesDTO
            };
        }
    }
}
