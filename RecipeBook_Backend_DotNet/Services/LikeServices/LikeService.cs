using Microsoft.EntityFrameworkCore;
using RecipeBook_Backend_DotNet.DTOs.LikeDTOs;
using RecipeBook_Backend_DotNet.DTOs.RecipeDTOs;
using RecipeBook_Backend_DotNet.DTOs.UserDTOs;

namespace RecipeBook_Backend_DotNet.Services.LikeServices
{
    public class LikeService : ILikeService
    {
        private readonly DataContext _context;

        public LikeService(DataContext context) {  
            _context = context;
        }

        public async Task<LikeMinimalDTO?> LikeOrDislike(LikeCreateDTO request)
        {
            var like = await _context.Likes.
                FirstOrDefaultAsync(like => 
                    like.RecipeId == request.RecipeId 
                    && 
                    like.UserId == request.UserId
                );
            if ( like == null )
            {
                var recipe = await _context.Recipes.FindAsync( request.RecipeId );
                var user = await _context.Users.FindAsync( request.UserId );

                if ( recipe != null && user != null )
                {
                    like = new Like
                    {
                        Recipe = recipe,
                        User = user
                    };
                    _context.Likes.Add(like);
                    await _context.SaveChangesAsync();
                    /*return new LikeMinimalDTO
                    {
                        Id = like.Id,
                        Recipe = new RecipeMinimalDTO { Id = like.RecipeId },
                        User = new UserMinimalDTO { Id = like.UserId }
                    };*/
                }
                else
                {
                    return null;
                }
            }
            else
            {
                _context.Likes.Remove(like);
                await _context.SaveChangesAsync();
                /*return new LikeMinimalDTO
                {
                    Id = like.Id,
                    Recipe = new RecipeMinimalDTO { Id = like.RecipeId},
                    User = new UserMinimalDTO { Id = like.UserId}
                };*/

            }
            new LikeMinimalDTO
            {
                Id = like.Id,
                Recipe = new RecipeMinimalDTO { Id = like.RecipeId },
                User = new UserMinimalDTO { Id = like.UserId }
            };

            if (like is not null)
            {
                return new LikeMinimalDTO
                {
                    Id = like.Id,
                    Recipe = new RecipeMinimalDTO { Id = like.RecipeId },
                    User = new UserMinimalDTO { Id = like.UserId }
                };
            }
            else
                return null;
        }
    }
}
