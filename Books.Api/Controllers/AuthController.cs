using Books.Application.DTOs.UserDTOs;
using Books.Application.Interfaces.Repositories;
using Books.Application.Interfaces.Services;
using Books.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Books.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(IUserService _UserService, IJwtService _jwtService) : ControllerBase
    {
        [HttpPost("loginWithPassword")]
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
        [HttpPost("LoginWithRecoveryToken")]
        public async Task<IActionResult> LoginWithRecoveryToken([FromBody] UserLoginDto dto)
        {
            var token = await _UserService.LoginWithRecoveryTokenAsync(dto, Request.HttpContext.Connection.RemoteIpAddress.ToString());

            Response.Cookies.Append("refreshToken", token.Token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true, // тільки HTTPS
                SameSite = SameSiteMode.Strict,
                Expires = token.Expires
            });

            return Ok(new { refreshToken = token });
        }
        [HttpPut("{email}")]
        public async Task<IActionResult> CreateRecoveryToke([FromRoute] string email)
        {
            string token = await _UserService.CreateRecoveryTokenAsync(email);
            return Ok(token);
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
        [HttpPut("UpdatePassword{newPassword}")]
        [Authorize]
        public async Task<IActionResult> UpdatePassword([FromRoute] string newPassword)
        {
            var authHeader = HttpContext.Request.Headers["Authorization"].ToString();
            var token = authHeader.Substring("Bearer ".Length).Trim();
            var principal = _jwtService.DecodeToken(token);
            if (principal == null) return Unauthorized();
            var email = principal.FindFirst(ClaimTypes.Email)?.Value;
            if (email == null) return Unauthorized();
            await _UserService.UpdatePasswordAsync(email, newPassword);
            return Ok();

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
