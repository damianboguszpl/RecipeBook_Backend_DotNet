using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecipeBook_Backend_DotNet.DTOs.CommentDTOs;
using RecipeBook_Backend_DotNet.Services.CommentServices;

namespace RecipeBook_Backend_DotNet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;

        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpGet("recipe/{id}")]
        public async Task<ActionResult<List<CommentPackedDTO>>> GetAllCommentsByRecipe(int id)
        {
            var result = await _commentService.GetAllCommentsByRecipe(id);
            if (result is null)
            {
                return NotFound("No comments found.");
            }
            return Ok(result);
        }

        [HttpPost, Authorize]
        public async Task<ActionResult<CommentMinimalDTO>> AddComment(CommentCreateDTO request)
        {
            var result = await _commentService.AddComment(request);
            if (result is null)
            {
                return BadRequest("New comment not added.");
            }
            return Ok(result);
        }

        [HttpDelete("{id}"), Authorize]
        public async Task<ActionResult<CommentMinimalDTO>> DeleteComment(int id)
        {
            var result = await _commentService.DeleteComment(id);
            if (result is null)
            {
                return NotFound("This comment doesn't exist.");
            }
            return Ok(result);

        }
    }
}
