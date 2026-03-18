using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Books.Domain.Entities;

namespace Books.Application.Interfaces.Repositories
{
    public interface IGenreRepository
    {
        Task<ICollection<GenreEntity>> GetAllGenresAsync();
        Task<GenreEntity> GetGenreByIdAsync(int id);
        Task<int?> AddGenreAsync(GenreEntity genre);
        Task DeleteGenreAsync(int id);
        Task<GenreEntity> UpdateGenreAsync(GenreEntity newGenre);
    }
}
