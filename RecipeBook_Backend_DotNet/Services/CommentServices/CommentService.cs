using RecipeBook_Backend_DotNet.DTOs.CommentDTOs;
using RecipeBook_Backend_DotNet.DTOs.RecipeDTOs;
using RecipeBook_Backend_DotNet.DTOs.UserDTOs;
using System.Security.Claims;

namespace RecipeBook_Backend_DotNet.Services.CommentServices
{
    public class CommentService : ICommentService
    {
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CommentService(DataContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<CommentMinimalDTO?> AddComment(CommentCreateDTO request)
        {
            var recipe = await _context.Recipes.FindAsync(request.RecipeId);
            if (recipe is null)
                return null;


            var user = await _context.Users.FindAsync(request.UserId);
            if (user is null)
                return null;
            else if (_httpContextAccessor.HttpContext != null)
            {
                var currentUserRole = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Role);
                if (currentUserRole != "admin")
                {
                    var currentUserId = _httpContextAccessor.HttpContext.User.FindFirstValue("Id");
                    if (currentUserId != request.UserId.ToString())
                    {
                        return null; // Forbidden: user has no rights to add comment as someone else
                    }
                }
            }

            var newComment = new Comment { 
                Text = request.Text,
                Username = user.Username,
                User = user,
                Recipe = recipe
            };

            _context.Comments.Add(newComment);
            await _context.SaveChangesAsync();

            var userDTO = new UserMinimalDTO
            {
                Id = user.Id,
                Username = user.Username
            };

            var commentDTO = new CommentMinimalDTO
            {
                Id = newComment.Id,
                Text = newComment.Text,
                User = userDTO
            };
            return commentDTO;
        }

        public async Task<CommentMinimalDTO?> DeleteComment(int id)
        {
            var comment = await _context.Comments.FindAsync(id);

            if (comment is null)
                return null;
            else
            {
                if(_httpContextAccessor.HttpContext != null)
                {
                    var currentUserRole = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Role);
                    if (currentUserRole != "admin")
                    {
                        var currentUserId = _httpContextAccessor.HttpContext.User.FindFirstValue("Id");
                        if(currentUserId != comment.UserId.ToString())
                        {
                            return null; // Forbidden: user has no rights to delete this comment because it's not his comment
                        }
                    }
                }

                _context.Comments.Remove(comment);
                await _context.SaveChangesAsync();
            }

            var user = await _context.Users.FindAsync(comment.UserId);

            if (user is null)
                return null;

            var userDTO = new UserMinimalDTO
            {
                Id = user.Id,
                Username = user.Username
            };

            var commentDTO = new CommentMinimalDTO
            {
                Id = comment.Id,
                Text = comment.Text,
                User = userDTO
            };
            return commentDTO;
        }

        public async Task<List<CommentPackedDTO>?> GetAllCommentsByRecipe(int id)
        {
            var recipe = await _context.Recipes
                .Include(c => c.User)
                .Include(c => c.Comments)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (recipe is null)
                return null;
            else if (recipe.Visibility == "private")
            {
                if (_httpContextAccessor.HttpContext != null)
                {
                    var currentUserRole = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Role);
                    if (currentUserRole != "admin")
                    {
                        var currentUserId = _httpContextAccessor.HttpContext.User.FindFirstValue("Id");
                        if (currentUserId != recipe.UserId.ToString())
                        {
                            return null; // Forbidden: user has no rights to get someone else's private recipe's comments
                        }

                    }
                }
                else
                    return null;
            }

            RecipeMinimalDTO recipeDTO = new() { Id = id };

            UserMinimalDTO userDTO = new(){ 
                Id = recipe.User.Id, 
                Username = recipe.User.Username
            };

            List<CommentPackedDTO> commentsDTO = recipe.Comments?.Select(comment => new CommentPackedDTO
            {
                Id = comment.Id,
                Text = comment.Text,
                User = userDTO,
                Recipe = recipeDTO
            }).ToList() ?? new List<CommentPackedDTO>();

            return commentsDTO;
        }
    }
}
