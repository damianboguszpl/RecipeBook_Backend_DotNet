namespace RecipeBook_Backend_DotNet.DTOs.AuthDTOs
{
    public record struct RefreshResponseDTO(
        int Code,
        string Info,
        string Token,
        string RefreshToken
        );
}
