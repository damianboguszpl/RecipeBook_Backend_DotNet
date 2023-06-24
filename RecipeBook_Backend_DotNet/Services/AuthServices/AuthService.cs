using Azure;
using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.IdentityModel.Tokens;
using RecipeBook_Backend_DotNet.DTOs.AuthDTOs;
using RecipeBook_Backend_DotNet.DTOs.RefreshTokenDTOs;
using RecipeBook_Backend_DotNet.DTOs.UserDTOs;
using RecipeBook_Backend_DotNet.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace RecipeBook_Backend_DotNet.Services.AuthServices
{
    public class AuthService : IAuthService
    {
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthService(DataContext context, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
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

            var refreshToken = SetRefreshToken(user);
            
            return new LoginResponseDTO
            {
                Code = 200,
                Info = "Logged in",
                User = new UserMinimalDTO
                {
                    Id = user.Id,
                    Username = user.Username
                },
                Token = token,
                RefreshToken = new RefreshTokenMinimalDTO
                {
                    Id = refreshToken.Result.Id,
                    Token = refreshToken.Result.Token,
                    Created = refreshToken.Result.Created,
                    Expires = refreshToken.Result.Expires
                }
            };
        }

        private static RefreshToken CreateRefreshToken(User user)
        {
            var refreshToken = new RefreshToken
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                Expires = DateTime.Now.AddDays(7),
                Created = DateTime.Now,
                User = user
            };

            return refreshToken;
        }

        private async Task<RefreshToken> SetRefreshToken(User user)
        {
            var existingRefreshToken = await _context.RefreshTokens.FindAsync(user.RefreshTokenId);

            if (existingRefreshToken != null)
            {
                existingRefreshToken.Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
                existingRefreshToken.Created = DateTime.Now;
                existingRefreshToken.Expires = DateTime.Now.AddDays(7);

                await _context.SaveChangesAsync();

                return existingRefreshToken;
            }
            else
            {
                RefreshToken newRefreshToken = CreateRefreshToken(user);
                _context.RefreshTokens.Add(newRefreshToken);

                await _context.SaveChangesAsync();

                user.RefreshToken = newRefreshToken;
                await _context.SaveChangesAsync();

                return newRefreshToken;
            }
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

        public async Task<LoginResponseDTO?> RefreshToken(RefreshRequestDTO request)
        {
            var user = await _context.Users
                .Include(u => u.RefreshToken)
                .FirstOrDefaultAsync(u => u.RefreshToken.Token == request.Token);

            if (user != null && user.RefreshToken != null)
            {
                if (_httpContextAccessor.HttpContext != null)
                {
                    var currentUserId = _httpContextAccessor.HttpContext.User.FindFirstValue("Id");
                    if (currentUserId != user.Id.ToString())
                    {
                        return new LoginResponseDTO
                        {
                            Code = 401,
                            Info = "Refresh Token invalid.", // Not this (logged in) user's refresh token !
                            Token = request.Token,
                            User = new UserMinimalDTO { Id = user.Id, Username = user.Username }
                        };
                    }
                }

                if (user.RefreshToken.Expires < DateTime.Now)
                {
                    return new LoginResponseDTO
                    {
                        Code = 401,
                        Info = "Refresh Token expired.",
                        Token = request.Token,
                        User = new UserMinimalDTO { Id = user.Id, Username = user.Username }
                    };
                }
                else
                {
                    string token = CreateToken(user);

                    var refreshToken = SetRefreshToken(user);

                    return new LoginResponseDTO
                    {
                        Code = 200,
                        Info = "Refresh Token refreshed.",
                        User = new UserMinimalDTO
                        {
                            Id = user.Id,
                            Username = user.Username
                        },
                        Token = token,
                        RefreshToken = new RefreshTokenMinimalDTO
                        {
                            Id = refreshToken.Result.Id,
                            Token = refreshToken.Result.Token,
                            Created = refreshToken.Result.Created,
                            Expires = refreshToken.Result.Expires
                        }
                    };
                }
            }
            else
            {
                return new LoginResponseDTO
                {
                    Code = 401,
                    Info = "Invalid Refresh Token.",
                    Token = request.Token,
                    User = null
                };
            }
            
        }
    }
}
