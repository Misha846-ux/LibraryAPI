using Books.Application.DTOs.UserDTOs;
using Books.Application.Interfaces.Repositories;
using Books.Infrastructure.Configuration;
using Books.Infrastructure.Services;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Moq;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace Books.Tests;

public class JwtServiceTest
{
    //private readonly ITestOutputHelper _output;
    private ITestOutputHelper _output;
    public JwtServiceTest(ITestOutputHelper output)
    {
        _output = output;
    }

    [Fact]
    public void JwtService_GenerateAccessToken_ShouldReturnValidJwtToken_WithCorrectClaims()
    {
        

        // ================================
        //   ARRANGE (підготовка)
        // ================================

        // Створюємо фейкові налаштування JWT
        // Вони використовуються для підпису токена
        var jwtSettings = Options.Create(new JwtSettings
        {
            Key = "super_secret_key_123456789_super_secret", // secret key
            Issuer = "test_issuer",
            Audience = "test_audience",
            ExpiresMinutes = 15
        });

        // Мокаємо репозиторії (ми тут їх НЕ використовуються, але потрібні в конструкторі)
        var refreshTokenRepoMock = new Mock<IRefreshTokenRepository>();
        var userRepoMock = new Mock<IUserRepository>();

        // Створюємо сервіс
        var jwtService = new JwtService(
            jwtSettings);

        // Дані користувача
        var loginDto = new UserLoginDto
        {
            Email = "myadmin@example.com"
        };

        var role = "Admin";

        // ================================
        // ACT (виклик методу)
        // ================================

        var tokenString = jwtService.GenerateAccessToken(loginDto, role);

        // ================================
        // ASSERT (перевірки)
        // ================================
        _output.WriteLine(tokenString);
        // 1. Перевіряємо що токен взагалі створився
        Assert.False(string.IsNullOrEmpty(tokenString));

        //Зчитування та валідація токена
        // 2. Створюємо handler щоб прочитати JWT
        var handler = new JwtSecurityTokenHandler();

        // 3. Парсимо токен (без валідації підпису!)
        var token = handler.ReadJwtToken(tokenString);

        // --------------------------------
        // Перевірка CLAIMS
        // --------------------------------

        // Перевіряємо Email
        var emailClaim = token.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.Email);
        Assert.NotNull(emailClaim);
        Assert.Equal("myadmin@example.com", emailClaim.Value);

        // Перевіряємо Role
        var roleClaim = token.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.Role);
        Assert.NotNull(roleClaim);
        Assert.Equal("Admin", roleClaim.Value);

        // Перевіряємо JTI (унікальний ID токена)
        var jtiClaim = token.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti);
        Assert.NotNull(jtiClaim);
        Assert.False(string.IsNullOrEmpty(jtiClaim.Value));

        // --------------------------------
        // Перевірка METADATA
        // --------------------------------

        // Перевіряємо Issuer (Issuer повинен точно співпадати)
        Assert.Equal("test_issuer", token.Issuer);

        // Перевіряємо Audience (Audience може бути масивом → перевірка через Contains)
        Assert.Contains("test_audience", token.Audiences);

        // --------------------------------
        // Перевірка EXPIRATION
        // --------------------------------

        // Перевіряємо що токен має expiration
        Assert.True(token.ValidTo > DateTime.UtcNow);

        // Перевіряємо що expiration приблизно правильний (±1 хв)
        var expectedExpiration = DateTime.UtcNow.AddMinutes(15);
        Assert.True(token.ValidTo <= expectedExpiration.AddMinutes(1));

    }
}