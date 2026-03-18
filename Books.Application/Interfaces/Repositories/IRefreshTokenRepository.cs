using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Books.Domain.Entities;

namespace Books.Application.Interfaces.Repositories
{
    public interface IRefreshTokenRepository
    {
        Task<int?> AddRefreshTokenAsync(RefreshTokenEntity token);
        Task<RefreshTokenEntity> GetRefreshTokenByIdAsync(int id);
        Task<RefreshTokenEntity> GetRefreshTokenByTokenAsync(string token);
        Task<ICollection<RefreshTokenEntity>> GetAllRefreshTokenAsync();
        Task<bool> UpdateRefreshTokenAsync();
    }
}
