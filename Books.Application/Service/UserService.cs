using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Books.Application.DTOs.UserDTOs;
using Books.Application.Interfaces.Healpers;
using Books.Application.Interfaces.Repositories;
using Books.Application.Interfaces.Services;
using Books.Domain.Entities;

namespace Books.Application.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;
        private readonly IMapper _mapper;
        private readonly IJwtService _jwtService;
        private readonly IHashHelper _hashHelper;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        public UserService(IUserRepository repository, IMapper mapper, IJwtService jwtService, IHashHelper hashHelper,
            IRefreshTokenRepository refreshTokenRepository)
        {
            this._repository = repository;
            this._mapper = mapper;
            this._jwtService = jwtService;
            this._hashHelper = hashHelper;
            this._refreshTokenRepository = refreshTokenRepository;
        }
        public async Task<string?> CreateUserAsync(UserCreateDto dto)
        {
            try
            {
                UserEntity user = this._mapper.Map<UserEntity>(dto);
                user.Email = user.Email.Trim();
                return await _repository.AddUserAsync(user, dto.Password);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<ICollection<UserReadDto>> GetAllUsersAsync()
        {
            try
            {
                ICollection<UserEntity> users = await _repository.GetAllUsersAsync();
                return _mapper.Map<ICollection<UserReadDto>>(users);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<UserReadDto?> GetUserByEmailAsync(string email)
        {
            try
            {
                UserEntity user = await _repository.GetUserByEmailAsync(email);
                return _mapper.Map<UserReadDto>(user);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<RefreshTokenEntity> LoginAsync(UserLoginDto dto, string ipAddress)
        {
            // Чи існує користувач з таким email
            var user = await _repository.GetUserByEmailAsync(dto.Email.Trim());
            if (user == null)
            {
                throw new UnauthorizedAccessException("Невірний email або пароль");
            }

            // Перевірка валідності пароля
            if (!_hashHelper.Check(dto.Password, user.PasswordHash))
            {
                throw new UnauthorizedAccessException("Неверный логин или пароль");

            }
            if (!user.IsActive)
            {
                throw new UnauthorizedAccessException("Користувача заблоковано");

            }

            // Генеруємо JWT токен для авторизованого користувача
            var token = _jwtService.GenerateRefreshToken(ipAddress, user.Id);
            await _refreshTokenRepository.AddRefreshTokenAsync(token);

            // Повертаємо токен
            return token;
        }

        public async Task<bool> LogOutAsync(string token)
        {
            RefreshTokenEntity tokenEntity = await _refreshTokenRepository.GetRefreshTokenByTokenAsync(token);
            if(tokenEntity == null)
            {
                throw new UnauthorizedAccessException("RefreshToken не найден");
            }
            tokenEntity.Expires = DateTime.UtcNow;
            await _refreshTokenRepository.UpdateRefreshTokenAsync();
            return true;
        }

        public async Task<string> RefreshAsync(string token)
        {
            RefreshTokenEntity tokenEntity = await _refreshTokenRepository.GetRefreshTokenByTokenAsync(token);

            if(tokenEntity == null)
            {
                throw new UnauthorizedAccessException("RefreshToken не найден");
            }

            if (tokenEntity.Expires <= DateTime.UtcNow)
            {
                throw new UnauthorizedAccessException("RefreshToken просрочен");
            }

            UserLoginDto dto = _mapper.Map<UserLoginDto>(tokenEntity.User);

            return _jwtService.GenerateAccessToken(dto, tokenEntity.User.Role.ToString());

        }
    }
}
