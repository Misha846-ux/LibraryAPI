using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Books.Application.Interfaces.Repositories;
using Books.Domain.Entities;
using Books.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Books.Infrastructure.Repositories
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly LibraryDbContext _context;

        public RefreshTokenRepository(LibraryDbContext context)
        {
            this._context = context;
        }
        public async Task<int?> AddRefreshTokenAsync(RefreshTokenEntity token)
        {
            try
            {
                await _context.RefreshTokens.AddAsync(token);
                _context.SaveChanges();
                return token.Id;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<ICollection<RefreshTokenEntity>> GetAllRefreshTokenAsync()
        {
            try
            {
                return await _context.RefreshTokens
                .Include(r => r.User)
                .ToListAsync();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<RefreshTokenEntity> GetRefreshTokenByIdAsync(int id)
        {
            try
            {
                return await _context.RefreshTokens
                    .Include(r => r.User)
                    .SingleOrDefaultAsync(r => r.Id == id);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<RefreshTokenEntity> GetRefreshTokenByTokenAsync(string token)
        {
            try
            {
                return await _context.RefreshTokens
                    .Include(r => r.User)
                    .SingleOrDefaultAsync(r => r.Token == token);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<bool> UpdateRefreshTokenAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
