using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RecipeBook_Backend_DotNet.DTOs.AuthDTOs;
using RecipeBook_Backend_DotNet.DTOs.UserDTOs;
using RecipeBook_Backend_DotNet.Services.AuthServices;
using RecipeBook_Backend_DotNet.Services.UserServices;

namespace RecipeBook_Backend_DotNet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
       public async Task<ActionResult<AuthResponseDTO>?> Register(UserRegisterDTO request)
        {
            var result = await _authService.Register(request);
            if (result is not null)
            {
                switch(result.Value.Code)
                {
                    case 200 : return Ok(result);
                    case 400: return BadRequest(result);
                }
            }
            return BadRequest();

        }

        [HttpPost("login")]
        public async Task<ActionResult<UserMinimalDTO>?> Login(UserLoginDTO request)
        {
            var result = await _authService.Login(request);
            if (result is not null)
            {
                switch (result.Value.Code)
                {
                    case 200: {
                            var cookieOptions = new CookieOptions
                            {
                                HttpOnly = true,
                                Expires = result.Value.RefreshToken.Value.Expires
                            };
                            Response.Cookies.Append("refreshToken", result.Value.RefreshToken.Value.Token, cookieOptions);

                            return Ok(result);
                    };
                    case 400: return BadRequest(result);
                }
            }
            return BadRequest();
            
        }

        [HttpPost("refresh-token"), Authorize]
        public async Task<ActionResult<string>?> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            if (refreshToken != null)
            {
                var request = new RefreshRequestDTO { Token = refreshToken };

                var result = await _authService.RefreshToken(request);
                if (result is not null)
                {
                    switch (result.Value.Code)
                    {
                        case 200:
                            {
                                var cookieOptions = new CookieOptions
                                {
                                    HttpOnly = true,
                                    Expires = result.Value.RefreshToken.Value.Expires
                                };
                                Response.Cookies.Append("refreshToken", result.Value.RefreshToken.Value.Token, cookieOptions);

                                return Ok(result);
                            };
                        case 400: return BadRequest(result);
                        case 401: return Unauthorized(result);
                    }
                }
                
            }
            return BadRequest();

        }
    }
}
