using RecipeBook_Backend_DotNet.DTOs.LikeDTOs;
using RecipeBook_Backend_DotNet.DTOs.RecipeDTOs;
using RecipeBook_Backend_DotNet.DTOs.UserDTOs;
using System.Security.Claims;

namespace RecipeBook_Backend_DotNet.Services.UserServices
{
    public class UserService : IUserService
    {
        private readonly DataContext _context;

        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(DataContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<UserPackedDTO?> GetUser(int id)
        {
            var user = await _context.Users
                .Include(u => u.Recipes)
                .Include(u => u.Likes)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user is null)
                return null;
            else if (_httpContextAccessor.HttpContext != null)
            {
                var currentUserRole = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Role);
                if (currentUserRole != "admin")
                {
                    var currentUserId = _httpContextAccessor.HttpContext.User.FindFirstValue("Id");
                    if (currentUserId != user.Id.ToString())
                    {
                        return null; // Forbidden: user has no rights to get other user's data
                    }
                }
            }

            UserMinimalDTO userDTO = new() { Id = user.Id, Username = user.Username };

            List<RecipeMinimalDTO> recipesDTO = user.Recipes?.Select(recipe => new RecipeMinimalDTO
            {
                Id = recipe.Id
            }).ToList() ?? new List<RecipeMinimalDTO>();

            List<LikeMinimalDTO> likesDTO = user.Likes?.Select(like => new LikeMinimalDTO
            {
                Id = like.Id,
                UserId = like.UserId,
                RecipeId = like.RecipeId
            }).ToList() ?? new List<LikeMinimalDTO>();

            return new UserPackedDTO
            {
                Id = user.Id,
                Username = user.Username,
                PermissionLevel = user.PermissionLevel,
                Recipes = recipesDTO,
                Likes = likesDTO                // data redundancy with this DTO... TODO: fix
            };
        }
    }
}
