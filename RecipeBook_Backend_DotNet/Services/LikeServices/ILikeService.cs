using RecipeBook_Backend_DotNet.DTOs.LikeDTOs;

namespace RecipeBook_Backend_DotNet.Services.LikeServices
{
    public interface ILikeService
    {
        Task<LikeMinimalDTO?> LikeOrDislike(LikeCreateDTO request);
    }
}
