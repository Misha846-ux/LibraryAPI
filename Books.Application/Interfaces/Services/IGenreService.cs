using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Books.Application.DTOs.GenreDTOs;

namespace Books.Application.Interfaces.Services
{
    public interface IGenreService
    {
        Task<int?> CreateGenreAsync(GenreCreateDto dto, CancellationToken cancellationToken);
        Task<GenreReadDto?> GetGenreByIdAsync(int id);
        Task<ICollection<GenreReadDto>> GetAllGenresAsync(CancellationToken cancellationToken);
        Task Delete(int id);
        Task<GenreReadDto> Update(int id,GenreCreateDto genreCreateDto);
    }
}
