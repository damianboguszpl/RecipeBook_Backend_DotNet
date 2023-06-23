namespace RecipeBook_Backend_DotNet.DTOs.UserDTOs
{
    public record struct UserLoginDTO(
        string Username,
        string Password
        );
}
