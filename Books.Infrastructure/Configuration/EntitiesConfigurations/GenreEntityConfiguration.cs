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
    public class GenreEntityConfiguration : IEntityTypeConfiguration<GenreEntity>
    {
        public void Configure(EntityTypeBuilder<GenreEntity> builder)
        {
            builder.HasKey(g => g.Id);

            builder.HasMany(g => g.Books)
                .WithOne(g => g.Genre)
                .HasForeignKey(g => g.GenreId);
        }
    }
}
