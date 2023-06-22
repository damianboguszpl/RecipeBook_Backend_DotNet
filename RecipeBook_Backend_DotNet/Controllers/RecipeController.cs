using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RecipeBook_Backend_DotNet.DTOs.RecipeDTOs;
using RecipeBook_Backend_DotNet.Models;
using RecipeBook_Backend_DotNet.Services.RecipeServices;
using System.Net;
using System.Xml.Linq;

namespace RecipeBook_Backend_DotNet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecipeController : ControllerBase
    {
        private readonly IRecipeService _recipeService;
        public RecipeController(IRecipeService recipeService)
        {
            _recipeService = recipeService;
        }

        [HttpGet]
        public async Task<ActionResult<List<RecipeMinimalDTO>>> GetAllRecipes()
        {
            var result = await _recipeService.GetAllRecipes();
            if (result is null)
            {
                return NotFound("No recipes found.");
            }
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RecipePackedDTO>> GetRecipe(int id)
        {
            var result = await _recipeService.GetRecipe(id);
            if (result is null)
            {
                return NotFound("This recipe doesn't exist.");
            }
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<RecipeMinimalDTO>> AddRecipe(RecipeCreateDto request)
        {
            var result = await _recipeService.AddRecipe(request);
            if (result is null)
            {
                return BadRequest("New recipe not added.");
            }
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<RecipeMinimalDTO>> UpdateRecipe(int id, RecipeUpdateDTO request)
        {
            var result = await _recipeService.UpdateRecipe(id, request);
            if (result is null)
            {
                return NotFound("This recipe doesn't exist.");
            }
            
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<RecipeMinimalDTO>> DeleteRecipe(int id)
        {
            var result = await _recipeService.DeleteRecipe(id);
            if (result is null)
            {
                return NotFound("This recipe doesn't exist.");
            }
            return Ok(result);

        }
    }
}