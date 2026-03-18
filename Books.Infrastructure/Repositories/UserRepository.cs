using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Books.Application.Interfaces.Healpers;
using Books.Application.Interfaces.Repositories;
using Books.Domain.Entities;
using Books.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Books.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly LibraryDbContext _context;
        private readonly IHashHelper _hashHelper;
        public UserRepository(LibraryDbContext context, IHashHelper hashHelper)
        {
            this._context = context;
            this._hashHelper = hashHelper;
        }
        public async Task<string?> AddUserAsync(UserEntity user, string password)
        {
            try
            {
                user.PasswordHash = _hashHelper.HashPassword(password);
                if(user.PasswordHash == null)
                {
                    throw new Exception("No password");
                }
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();
                return user.Email;
            }
            catch
            {
                return null;
            }
        }

        public async Task<ICollection<UserEntity>> GetAllUsersAsync()
        {
            try
            {
                return await _context.Users
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<UserEntity> GetUserByEmailAsync(string email)
        {
            try
            {
                return await _context.Users
                    .SingleOrDefaultAsync(u => u.Email == email);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
