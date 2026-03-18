using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Books.Domain.Entities;

namespace Books.Application.Interfaces.Repositories
{
    public interface IAuthorRepository
    {
        Task<ICollection<AuthorEntity>> GetAllAuthorsAsync(CancellationToken cancellationToken);
        Task<AuthorEntity> GetAuthorByIdAsync(int id);
        Task<int?> AddAuthorAsync(AuthorEntity author, CancellationToken cancellationToken);
        Task DeleteAuthorAsync(int id, CancellationToken cancellationToken);
        Task<AuthorEntity> UpdateAuthorAsync(AuthorEntity newAuthor, CancellationToken cancellationToken);
    }
}
