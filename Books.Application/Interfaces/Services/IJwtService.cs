using Books.Application.DTOs.UserDTOs;
using Books.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Books.Application.Interfaces.Services
{
    public interface IJwtService
    {
        string GenerateAccessToken(UserLoginDto userLoginDto, string role);

        RefreshTokenEntity GenerateRefreshToken(string ipAddress, Guid userId);
        ClaimsPrincipal? DecodeToken(string token);
    }
}
