using Azure.Core;
using RecipeBook_Backend_DotNet.DTOs.CategoryDTOs;
using RecipeBook_Backend_DotNet.DTOs.IngredientDTOs;
using RecipeBook_Backend_DotNet.DTOs.LikeDTOs;
using RecipeBook_Backend_DotNet.DTOs.RecipeDTOs;
using RecipeBook_Backend_DotNet.DTOs.UserDTOs;
using RecipeBook_Backend_DotNet.Models;

namespace RecipeBook_Backend_DotNet.Services.UserServices
{
    public class UserService : IUserService
    {
        private readonly DataContext _context;

        public UserService(DataContext context)
        {
            _context = context;
        }

        public async Task<UserMinimalDTO?> AddUser(UserCreateDTO request)
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
        }

        public async Task<UserPackedDTO?> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user is null)
                return null;

            var recipes = await _context.Recipes.
                Where(recipe => recipe.UserId == user.Id)
                .ToListAsync();

            List<RecipeMinimalDTO> recipesDTO = recipes.Select(recipe => new RecipeMinimalDTO
            {
                Id = recipe.Id
            }).ToList();

            var likes = await _context.Likes
                    .Where(like => like.UserId == user.Id)
                    .ToListAsync();

            UserMinimalDTO userDTO = new() { Id = user.Id, Username = user.Username };

            List<LikeMinimalDTO> likesDTO = new();

            foreach (var like in likes)
            {
                var recipe = _context.Recipes.
                    FirstOrDefaultAsync(recipe => recipe.Id == like.RecipeId);
                RecipeMinimalDTO recipeDTO = new() {
                    Id = recipe.Id
                };

                likesDTO.Add( new LikeMinimalDTO
                {
                    Id = like.Id,
                    User = userDTO,
                    Recipe = recipeDTO
                });
            }

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
