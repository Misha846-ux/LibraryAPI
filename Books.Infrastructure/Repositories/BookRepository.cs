using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Azure.Core;
using Books.Application.DTOs.BookDTOs;
using Books.Application.Interfaces.Repositories;
using Books.Domain.Entities;
using Books.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Books.Infrastructure.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly LibraryDbContext _context;
        public BookRepository(LibraryDbContext context)
        {
            this._context = context;
        }
        public async Task<int?> AddBookAsync(BookEntity book, ICollection<int> authorsId)
        {
            try
            {
                if(authorsId != null)
                {
                    ICollection<AuthorEntity> authors = this._context.Authors.Where(x => authorsId.Contains(x.Id)).ToList();
                    book.Authors = authors;
                }
                this._context.Books.Add(book);
                this._context.SaveChanges();
                return book.Id;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task DeleteBookAsync(int id)
        {
            _context.Books.Remove(await GetBookByIdAsync(id));
            await _context.SaveChangesAsync();
        }

        public async Task<ICollection<BookEntity>> GetAllBooksAsync()
        {
            try
            {
                return await this._context.Books
                    .Include(book => book.Authors)
                    .Include(book => book.Genre)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<BookEntity> GetBookByIdAsync(int id)
        {
            try
            {
                return await this._context.Books
                    .Include(x => x.Authors)
                    .Include(x => x.Genre)
                    .SingleOrDefaultAsync(x => x.Id == id);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<ICollection<BookEntity>> GetBookFilteredAsync(int pageCount, int limit)
        {
            try
            {
                if(pageCount < 1)
                {
                    pageCount = 1;
                }
                ICollection<BookEntity> books = await this._context.Books
                    .Include(x => x.Authors)
                    .Include(x => x.Genre)
                    .OrderBy(x => x.Id)
                    .Skip((pageCount - 1) * limit)
                    .Take(limit)
                    .ToListAsync();
                return books;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<BookEntity> UpdateBookAsync(BookEntity newBook)
        {
            BookEntity book = await GetBookByIdAsync(newBook.Id);
            if(book == null)
            {
                throw new Exception("Book not find");
            }
            PropertyInfo[] properties = typeof(BookEntity).GetProperties();
            foreach(var prop in properties)
            {
                var value = prop.GetValue(newBook);
                if (value != null)
                {
                    prop.SetValue(book, value);
                }
            }
            await _context.SaveChangesAsync();
            return book;
        }
    }
}
