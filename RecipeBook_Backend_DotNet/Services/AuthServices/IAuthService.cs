using RecipeBook_Backend_DotNet.DTOs.AuthDTOs;
using RecipeBook_Backend_DotNet.DTOs.UserDTOs;

namespace RecipeBook_Backend_DotNet.Services.AuthServices
{
    public interface IAuthService
    {
        Task<AuthResponseDTO?> Register(UserRegisterDTO request);
        Task<LoginResponseDTO?> Login(UserLoginDTO request);
    }
}
