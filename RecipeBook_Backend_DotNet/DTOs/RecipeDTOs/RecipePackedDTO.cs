using RecipeBook_Backend_DotNet.DTOs.CategoryDTOs;
using RecipeBook_Backend_DotNet.DTOs.CommentDTOs;
using RecipeBook_Backend_DotNet.DTOs.IngredientDTOs;
using RecipeBook_Backend_DotNet.DTOs.LikeDTOs;
using RecipeBook_Backend_DotNet.DTOs.UserDTOs;

namespace RecipeBook_Backend_DotNet.DTOs.RecipeDTOs
{
    public record struct RecipePackedDTO(
        int Id,
        string Name,
        string RecipeDescription,
        string PrepareTime,
        string CookTime,
        int AuthorsRating,
        string PublishingStatus,
        string Visibility,
        UserMinimalDTO User,
        CategoryMinimalDTO Category,
        List<IngredientMinimalDTO> Ingredients,
        List<CommentMinimalDTO> Comments,
        List<LikeMinimalDTO> Likes
        );
}
