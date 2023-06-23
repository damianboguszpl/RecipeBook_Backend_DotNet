using RecipeBook_Backend_DotNet.DTOs.LikeDTOs;
using RecipeBook_Backend_DotNet.DTOs.RecipeDTOs;
using RecipeBook_Backend_DotNet.DTOs.UserDTOs;

namespace RecipeBook_Backend_DotNet.Services.UserServices
{
    public class UserService : IUserService
    {
        private readonly DataContext _context;

        public UserService(DataContext context)
        {
            _context = context;
        }

        /*public async Task<UserMinimalDTO?> AddUser(UserCreateDTO request)
        {
            if(request.Username is null || request.Password is null)
                return null;

            var user = await _context.Users.
                FirstOrDefaultAsync(user => user.Username == request.Username);

            if (user == null)
            {
                var newUser = new User
                {
                    Username = request.Username,
                    Password = request.Password,
                    PermissionLevel = "user"
                };
                _context.Users.Add(newUser);
                await _context.SaveChangesAsync();

                var userDTO = new UserMinimalDTO
                {
                    Id = newUser.Id,
                    Username = newUser.Username
                };
                return userDTO;
            }
            else
                return null;
        }*/

        public async Task<UserPackedDTO?> GetUser(int id)
        {
            var user = await _context.Users
                .Include(u => u.Recipes)
                .Include(u => u.Likes)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user is null)
                return null;

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
