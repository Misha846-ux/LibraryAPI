using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Books.Domain.Entities;

namespace Books.Application.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<ICollection<UserEntity>> GetAllUsersAsync();
        Task<UserEntity> GetUserByEmailAsync(string email);
        Task<string?> AddUserAsync(UserEntity user, string password);
        Task SaveChengesAsync();
    }
}
