using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Books.Application.DTOs.BookDTOs;
using Books.Domain.Entities;

namespace Books.Application.Interfaces.Repositories
{
    public interface IBookRepository
    {
        /// <summary>
        /// Получение всех книг из БД
        /// </summary>
        /// <returns></returns>
        Task<ICollection<BookEntity>> GetAllBooksAsync();

        /// <summary>
        /// Получение одной кники по id из БД
        /// </summary>
        /// <param name="id">индефикатор книги которую нобходимо получить</param>
        /// <returns></returns>
        Task<BookEntity> GetBookByIdAsync(int id);

        /// <summary>
        /// Добавляет в БД книгу и возращает индефикатор книги или null если операция неудачна
        /// </summary>
        /// <param name="book"></param>
        /// <returns></returns>
        Task<int?> AddBookAsync(BookEntity book, ICollection<int> authorsId);

        Task<ICollection<BookEntity>> GetBookFilteredAsync(int pageCount, int limit);
        Task DeleteBookAsync(int id);
        Task<BookEntity> UpdateBookAsync(BookEntity newBook);
    }
}
