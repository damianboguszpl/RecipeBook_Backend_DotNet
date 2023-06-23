using RecipeBook_Backend_DotNet.DTOs.LikeDTOs;

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
                    like.RecipeId == request.RecipeId  && like.UserId == request.UserId
                );

            if ( like == null )
            {
                var recipe = await _context.Recipes
                .FirstOrDefaultAsync(r => r.Id == request.RecipeId);

                var user = await _context.Users.FindAsync(request.UserId);

                if (recipe != null && user != null)
                {
                    like = new Like
                    {
                        Recipe = recipe,
                        User = user
                    };

                    _context.Likes.Add(like);
                    await _context.SaveChangesAsync();
                }
                else
                    return null;
            }
            else
            {
                _context.Likes.Remove(like);
                await _context.SaveChangesAsync();
            }

            return new LikeMinimalDTO
            {
                Id = like.Id,
                UserId = like.UserId,
                RecipeId = like.RecipeId
            };
        }
    }
}
