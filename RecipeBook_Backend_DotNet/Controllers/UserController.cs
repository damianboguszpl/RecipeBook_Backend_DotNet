using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecipeBook_Backend_DotNet.DTOs.IngredientDTOs;
using RecipeBook_Backend_DotNet.DTOs.UserDTOs;
using RecipeBook_Backend_DotNet.Services.UserServices;

namespace RecipeBook_Backend_DotNet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("{id}"), Authorize]
        public async Task<ActionResult<UserPackedDTO>> GetUser(int id)
        {
            var result = await _userService.GetUser(id);
            if (result is null)
            {
                return NotFound("This User doesn't exist.");
            }
            return Ok(result);
        }

        /*[HttpPost]
        public async Task<ActionResult<UserMinimalDTO>> AddUser(UserCreateDTO request)
        {
            var result = await _userService.AddUser(request);
            if (result is null)
            {
                return BadRequest("New User not added.");
            }
            return Ok(result);

        }*/
    }
}
