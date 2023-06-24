namespace RecipeBook_Backend_DotNet.DTOs.RefreshTokenDTOs
{
    public record struct RefreshTokenMinimalDTO(
        int Id,
        string Token,
        DateTime Created,
        DateTime Expires
        );
}
