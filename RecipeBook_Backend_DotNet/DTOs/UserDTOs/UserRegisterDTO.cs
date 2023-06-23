namespace RecipeBook_Backend_DotNet.DTOs.UserDTOs
{
    /*public class UserRegisterDTO
    {
        public required string Username;
        public required string Password;
    }*/
    public record struct UserRegisterDTO(
        string Username,
        string Password
        );
}
