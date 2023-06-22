using RecipeBook_Backend_DotNet.DTOs.UserDTOs;

namespace RecipeBook_Backend_DotNet.Services.UserServices
{
    public interface IUserService
    {
        Task<UserPackedDTO?> GetUser(int id);
        Task<UserMinimalDTO?> AddUser(UserCreateDTO request);
    }
}
