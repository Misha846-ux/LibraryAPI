using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Books.Application.DTOs.UserDTOs;
using Books.Domain.Entities;

namespace Books.Application.Interfaces.Services
{
    public interface IJwtService
    {
        string GenerateAccessToken(UserLoginDto userLoginDto, string role);

        RefreshTokenEntity GenerateRefreshToken(string ipAddress, Guid userId);
    }
}
