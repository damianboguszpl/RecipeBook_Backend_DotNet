namespace RecipeBook_Backend_DotNet.DTOs.UserDTOs
{
    public record struct UserCreateDTO(
        string Username,
        string Password
        );
}
