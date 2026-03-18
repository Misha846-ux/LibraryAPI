using Books.Application.DTOs.UserDTOs;
using Books.Application.Interfaces.Repositories;
using Books.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Books.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(IUserService _UserService) : ControllerBase
    {
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto dto)
        {
            var token = await _UserService.LoginAsync(dto, Request.HttpContext.Connection.RemoteIpAddress.ToString());
            

            Response.Cookies.Append("refreshToken", token.Token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true, // тільки HTTPS
                SameSite = SameSiteMode.Strict,
                Expires = token.Expires
            });

            return Ok(new { refreshToken = token });
        }
        

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] UserCreateDto request)
        {
            string? userEmail = await _UserService.CreateUserAsync(request);
            return Ok(userEmail);
        }
        [HttpPost("Refresh")]
        public async Task<IActionResult> Refresh()
        {
            var token = Request.Cookies["refreshToken"];
            if (string.IsNullOrEmpty(token))
                return Unauthorized();
            var accessToken = await _UserService.RefreshAsync(token);
            return Ok(new {token = accessToken});
        }

        [HttpPost("LogOut")]
        public async Task<IActionResult> LogOut()
        {
            var token = Request.Cookies["refreshToken"];
            await _UserService.LogOutAsync(token);
            return Ok();
        }
        
    }
}
