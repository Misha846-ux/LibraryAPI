using Books.Application.DTOs.UserDTOs;
using Books.Application.Interfaces.Services;
using Books.Application.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Books.Api.Controllers
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/[controller]")]
    public class UserController(IUserService _UserService): ControllerBase
    {

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _UserService.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpGet("{email}")]
        public async Task<IActionResult> GetById([FromRoute] string email)
        {
            var genre = await _UserService.GetUserByEmailAsync(email);
            return Ok(genre);
        }

        
    }
}
