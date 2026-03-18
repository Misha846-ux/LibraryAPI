using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Books.Application.DTOs.BookDTOs;
using Books.Domain.Entities;

namespace Books.Application.Interfaces.Services
{
    public interface IBookService
    {
        /// <summary>
        /// Створює нову книгу разом з авторами і жанром
        /// </summary>
        /// <param name="dto">DTO для створення книги</param>
        /// <returns>Id створеної книги</returns>
        Task<int?> CreateBookAsync(BookCreateDto dto, CancellationToken cancellationToken);

        /// <summary>
        /// Повертає книгу по Id
        /// </summary>
        /// <param name="id">Id книги</param>
        /// <returns>BookReadDto або null, якщо не знайдено</returns>
        Task<BookReadDto?> GetBookByIdAsync(int id);

        /// <summary>
        /// Повертає всі книги
        /// </summary>
        /// <returns>Колекція BookReadDto</returns>
        Task<ICollection<BookReadDto>> GetAllBooksAsync(CancellationToken cancellationToken);

        Task<ICollection<BookReadDto>> GetBookFilteredAsync(int pageCount, int limit);
        Task Delete(int id);
        Task<BookReadDto> Update(int id, BookCreateDto bookCreateDto);
    }
}
