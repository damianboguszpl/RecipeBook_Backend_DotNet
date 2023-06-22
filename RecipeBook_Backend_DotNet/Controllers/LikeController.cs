using Microsoft.AspNetCore.Mvc;
using RecipeBook_Backend_DotNet.DTOs.LikeDTOs;
using RecipeBook_Backend_DotNet.Services.LikeServices;

namespace RecipeBook_Backend_DotNet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LikeController : ControllerBase
    {
        private readonly ILikeService _likeService;

        public LikeController(ILikeService likeService)
        {
            _likeService = likeService;
        }

        [HttpPost]
        public async Task<ActionResult<LikeMinimalDTO>> AddCategory(LikeCreateDTO request)
        {
            var result = await _likeService.LikeOrDislike(request);
            if (result is null)
            {
                return BadRequest("Can't like nor dislike.");
            }
            return Ok(result);
        }
    }
}
