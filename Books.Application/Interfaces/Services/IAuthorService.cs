using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Books.Application.DTOs.AuthorDTOs;

namespace Books.Application.Interfaces.Services
{
    public interface IAuthorService
    {
        Task<int?> CreateAuthorAsync(AuthorCreateDto dto, CancellationToken cancellationToken);
        Task<AuthorReadDto?> GetAuthorByIdAsync(int id);
        Task<ICollection<AuthorReadDto>> GetAllAuthorsAsync(CancellationToken cancellationToken);
        Task Delete(int id);
        Task<AuthorReadDto> Update(int id, AuthorCreateDto authorCreateDto);
    }
}
