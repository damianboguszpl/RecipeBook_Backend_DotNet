using Microsoft.AspNetCore.Mvc;
using RecipeBook_Backend_DotNet.DTOs.CategoryDTOs;
using RecipeBook_Backend_DotNet.Services.CategoryServices;

namespace RecipeBook_Backend_DotNet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<ActionResult<List<CategoryPackedDTO>>> GetAllCategories()
        {
            var result = await _categoryService.GetAllCategories();
            if (result is null)
            {
                return NotFound("No categories found.");
            }
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryPackedDTO>> GetCategory(int id)
        {
            var result = await _categoryService.GetCategory(id);
            if (result is null)
            {
                return NotFound("This category doesn't exist.");
            }
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<CategoryMinimalDTO>> AddCategory(CategoryCreateDTO request)
        {
            var result = await _categoryService.AddCategory(request);
            if (result is null)
            {
                return BadRequest("New category not added.");
            }
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<CategoryMinimalDTO>> DeleteCategory(int id)
        {
            var result = await _categoryService.DeleteCategory(id);
            if (result is null)
            {
                return NotFound("This category doesn't exist or can't be deleted.");
            }
            return Ok(result);

        }
    }
}
