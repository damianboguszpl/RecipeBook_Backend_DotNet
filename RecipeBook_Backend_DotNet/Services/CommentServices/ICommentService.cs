using RecipeBook_Backend_DotNet.DTOs.CommentDTOs;
using RecipeBook_Backend_DotNet.DTOs.IngredientDTOs;

namespace RecipeBook_Backend_DotNet.Services.CommentServices
{
    public interface ICommentService
    {
        Task<List<CommentPackedDTO>?> GetAllCommentsByRecipe(int id);
        Task<CommentMinimalDTO?> AddComment(CommentCreateDTO request);
        Task<CommentMinimalDTO?> DeleteComment(int id);
    }
}