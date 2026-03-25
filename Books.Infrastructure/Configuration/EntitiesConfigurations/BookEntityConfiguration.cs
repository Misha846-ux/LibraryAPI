using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Books.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Books.Infrastructure.Configuration.EntitiesConfigurations
{
    public class BookEntityConfiguration : IEntityTypeConfiguration<BookEntity>
    {
        public void Configure(EntityTypeBuilder<BookEntity> builder)
        {
            builder.HasKey(b => b.Id);

            builder.Property(b => b.CreatedAt)
                   .HasDefaultValueSql("SYSDATETIME()");

            builder.HasOne(b => b.Genre)
                   .WithMany(g => g.Books)
                   .HasForeignKey(b => b.GenreId);
            builder.HasMany(b => b.Authors)
                .WithMany(a => a.Books);
        }
    }
}
