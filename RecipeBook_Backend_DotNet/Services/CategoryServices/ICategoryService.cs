using RecipeBook_Backend_DotNet.DTOs.CategoryDTOs;

namespace RecipeBook_Backend_DotNet.Services.CategoryServices
{
    public interface ICategoryService
    {
        Task<List<CategoryPackedDTO>?> GetAllCategories();
        Task<CategoryPackedDTO?> GetCategory(int id);
        Task<CategoryMinimalDTO?> AddCategory(CategoryCreateDTO request);
        Task<CategoryMinimalDTO?> DeleteCategory(int id);
    }
}
