using RecipeBook_Backend_DotNet.DTOs.UserDTOs;

namespace RecipeBook_Backend_DotNet.DTOs.AuthDTOs
{
    public record struct LoginResponseDTO(
        int Code,
        string Info,
        UserMinimalDTO? User,
        string Token
        );
}
