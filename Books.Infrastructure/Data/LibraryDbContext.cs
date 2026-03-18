using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Books.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Books.Infrastructure.Data
{
    public class LibraryDbContext: DbContext
    {
        public DbSet<BookEntity> Books { get; set; }
        public DbSet<AuthorEntity> Authors { get; set; }
        public DbSet<GenreEntity> Genres { get; set; }
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<RefreshTokenEntity> RefreshTokens { get; set; }
        public DbSet<CountryEntity> Countries { get; set; }

        public LibraryDbContext(DbContextOptions<LibraryDbContext> options): base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserEntity>().HasIndex(u => u.Email).IsUnique();

            if (Database.IsMySql())
            {
                modelBuilder.Entity<BookEntity>()
                .Property(b => b.CreatedAt)
                .HasColumnType("datetime(6)")        // точность микросекунд
                .HasDefaultValueSql("CURRENT_TIMESTAMP(6)")
                .ValueGeneratedOnAdd();
            }
            else if (Database.IsSqlServer())
            {
                modelBuilder.Entity<BookEntity>()
                    .Property(b => b.CreatedAt)
                    .HasDefaultValueSql("SYSDATETIME()");
            }
           
        }
    }
}
