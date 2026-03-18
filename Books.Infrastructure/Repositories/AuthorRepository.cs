using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Books.Application.Interfaces.Repositories;
using Books.Domain.Entities;
using Books.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Books.Infrastructure.Repositories
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly LibraryDbContext _context;

        public AuthorRepository(LibraryDbContext context)
        {
            this._context = context;
        }
        public async Task<int?> AddAuthorAsync(AuthorEntity author, CancellationToken cancellationToken)
        {
            try
            {
                await this._context.Authors.AddAsync(author);
                await this._context.SaveChangesAsync(cancellationToken);
                return author.Id;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task DeleteAuthorAsync(int id, CancellationToken cancellationToken)
        {
            _context.Authors.Remove(await GetAuthorByIdAsync(id));
            await _context.SaveChangesAsync();
        }

        public async Task<ICollection<AuthorEntity>> GetAllAuthorsAsync(CancellationToken cancellationToken)
        {
            try
            {
                return await this._context.Authors
                    .Include(auhtor => auhtor.Books)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<AuthorEntity> GetAuthorByIdAsync(int id)
        {
            try
            {
                return await this._context.Authors
                    .Include(auhtor => auhtor.Books)
                    .SingleOrDefaultAsync(x => x.Id == id);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<AuthorEntity> UpdateAuthorAsync(AuthorEntity newAuthor, CancellationToken cancellationToken)
        {
            AuthorEntity author = await GetAuthorByIdAsync(newAuthor.Id);
            if (author == null)
            {
                throw new Exception("Author not found");
            }

            PropertyInfo[] properties = typeof(AuthorEntity).GetProperties();

            foreach (var prop in properties)
            {
                var value = prop.GetValue(newAuthor);
                if (value != null)
                {
                    prop.SetValue(author, value);
                }
            }

            await _context.SaveChangesAsync(cancellationToken);
            return author;
        }
    }
}
