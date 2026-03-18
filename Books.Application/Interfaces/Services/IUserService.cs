using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Books.Application.DTOs.UserDTOs;
using Books.Domain.Entities;

namespace Books.Application.Interfaces.Services
{
    public interface IUserService
    {
        Task<string?> CreateUserAsync(UserCreateDto dto);
        Task<UserReadDto?> GetUserByEmailAsync(string email);
        Task<ICollection<UserReadDto>> GetAllUsersAsync();
        Task<RefreshTokenEntity> LoginAsync(UserLoginDto dto, string ipAddress);

        Task<string> RefreshAsync(string token);
        Task<bool> LogOutAsync(string token);
    }
}
