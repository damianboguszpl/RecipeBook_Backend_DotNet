using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.IdentityModel.Tokens;
using RecipeBook_Backend_DotNet.DTOs.AuthDTOs;
using RecipeBook_Backend_DotNet.DTOs.UserDTOs;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RecipeBook_Backend_DotNet.Services.AuthServices
{
    public class AuthService : IAuthService
    {
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(DataContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<AuthResponseDTO?> Register(UserRegisterDTO request)
        {
            if (request.Username is null || request.Password is null)
                return new AuthResponseDTO { 
                    Code = 400,
                    Info = "Bad request.",
                    User = null
                };

            var user = await _context.Users.
                FirstOrDefaultAsync(user => user.Username == request.Username);

            if (user is null)
            {
                string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
                var newUser = new User
                {
                    Username = request.Username,
                    PasswordHash = passwordHash,
                    PermissionLevel = "user"
                };
                _context.Users.Add(newUser);
                await _context.SaveChangesAsync();

                var userDTO = new UserMinimalDTO
                {
                    Id = newUser.Id,
                    Username = newUser.Username
                };
                return new AuthResponseDTO
                {
                    Code = 200,
                    Info = "New User registered.",
                    User = userDTO
                };
            }
            else
                return new AuthResponseDTO
                {
                    Code = 400,
                    Info = "Username is already taken.",
                    User = null
                };
        }

        public async Task<LoginResponseDTO?> Login(UserLoginDTO request)
        {
            var user = await _context.Users.
                FirstOrDefaultAsync(user => user.Username == request.Username);

            if ( user is null )
            {
                return new LoginResponseDTO
                {
                    Code = 400,
                    Info = "Username or password is wrong.",
                    /*User = null,
                    token = ""*/
                };
            }
                

            if(!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                return new LoginResponseDTO
                {
                    Code = 400,
                    Info = "Username or password is wrong.",
                    /*User = null,
                    token = ""*/
                };
            }

            string token = CreateToken(user);

            return new LoginResponseDTO
            {
                Code = 200,
                Info = "Logged in",
                User = new UserMinimalDTO
                {
                    Id = user.Id,
                    Username = user.Username
                },
                Token = token
            };
        }

        private string CreateToken(User user)
        {
            List<Claim> claims = new()
            {
                new Claim("Id", user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.PermissionLevel)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:SigningKey").Value!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.Now.AddDays(1),
                    signingCredentials: creds
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }
    }
}
