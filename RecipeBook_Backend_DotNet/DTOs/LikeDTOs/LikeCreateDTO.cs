namespace RecipeBook_Backend_DotNet.DTOs.LikeDTOs
{
    public record struct LikeCreateDTO(
        int UserId,
        int RecipeId
        );
}
