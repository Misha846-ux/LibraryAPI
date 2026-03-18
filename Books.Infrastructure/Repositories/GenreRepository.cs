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
    public class GenreRepository : IGenreRepository
    {
        private readonly LibraryDbContext _context;

        public GenreRepository(LibraryDbContext context)
        {
            this._context = context;
        }
        public async Task<int?> AddGenreAsync(GenreEntity genre)
        {
            try
            {
                await this._context.Genres.AddAsync(genre);
                this._context.SaveChanges();
                return genre.Id;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task DeleteGenreAsync(int id)
        {
            _context.Genres.Remove(await GetGenreByIdAsync(id));
            await _context.SaveChangesAsync();
        }

        public async Task<ICollection<GenreEntity>> GetAllGenresAsync()
        {
            try
            {
                return await this._context.Genres
                    .Include(x => x.Books)
                    .ToListAsync(); ;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<GenreEntity> GetGenreByIdAsync(int id)
        {
            try
            {
                return await this._context.Genres
                    .Include(x => x.Books)
                    .SingleOrDefaultAsync(x => x.Id == id);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<GenreEntity> UpdateGenreAsync(GenreEntity newGenre)
        {
            GenreEntity genre = await GetGenreByIdAsync(newGenre.Id);
            if(genre == null)
            {
                throw new Exception("Genre not find");
            }
            PropertyInfo[] properties = typeof(GenreEntity).GetProperties();
            foreach(var prop in properties)
            {
                var value = prop.GetValue(newGenre);
                if (value != null)
                {
                    prop.SetValue(genre, value);
                }
            }
            await _context.SaveChangesAsync();
            return genre;
        }
    }
}
