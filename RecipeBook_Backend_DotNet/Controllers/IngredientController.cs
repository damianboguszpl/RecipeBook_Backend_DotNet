﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecipeBook_Backend_DotNet.DTOs.IngredientDTOs;
using RecipeBook_Backend_DotNet.Services.IngredientServices;

namespace RecipeBook_Backend_DotNet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IngredientController : ControllerBase
    {
        private readonly IIngredientService _ingredientService;

        public IngredientController(IIngredientService ingredientService)
        {
            _ingredientService = ingredientService;
        }

        [HttpGet, Authorize(Roles = "admin")]
        public async Task<ActionResult<List<IngredientPackedDTO>>> GetAllIngredients()
        {
            var result = await _ingredientService.GetAllIngredients();
            if (result is null)
            {
                return NotFound("No ingredients found.");
            }
            return Ok(result);
        }

        [HttpGet("public")]
        public async Task<ActionResult<List<IngredientPackedDTO>>> GetAllPublicIngredients()
        {
            var result = await _ingredientService.GetAllPublicIngredients();
            if (result is null)
            {
                return NotFound("No ingredients found.");
            }
            return Ok(result);
        }

        [HttpGet("recipe/{id}")]
        public async Task<ActionResult<List<IngredientPackedDTO>>> GetAllIngredientsByRecipe(int id)
        {
            var result = await _ingredientService.GetAllIngredientsByRecipe(id);
            if (result is null)
            {
                return NotFound("No ingredients found.");
            }
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IngredientPackedDTO>> GetIngredient(int id)
        {
            var result = await _ingredientService.GetIngredient(id);
            if (result is null)
            {
                return NotFound("This ingredient doesn't exist.");
            }
            return Ok(result);
        }

        [HttpPost, Authorize]
        public async Task<ActionResult<IngredientMinimalDTO>> AddIngredient(IngredientCreateDTO request)
        {
            var result = await _ingredientService.AddIngredient(request);
            if (result is null)
            {
                return BadRequest("New ingredient not added.");
            }
            return Ok(result);
        }

        [HttpDelete("{id}"), Authorize]
        public async Task<ActionResult<IngredientMinimalDTO>> DeleteIngredient(int id)
        {
            var result = await _ingredientService.DeleteIngredient(id);
            if (result is null)
            {
                return NotFound("This ingredient doesn't exist.");
            }
            return Ok(result);

        }
    }
}
