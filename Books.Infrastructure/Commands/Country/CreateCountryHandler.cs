using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Books.Domain.Entities;
using Books.Infrastructure.Data;
using MediatR;

namespace Books.Infrastructure.Commands.Country
{
    public class CreateCountryHandler:IRequestHandler<CreateCountryCommand, CountryEntity>
    {
        private LibraryDbContext _context;
        public CreateCountryHandler(LibraryDbContext context)
        {
            this._context = context;
        }

        public async Task<CountryEntity> Handle(CreateCountryCommand requst, CancellationToken cancellationToken)
        {
            CountryEntity country = new CountryEntity();
            country.Name = requst.name;
            await _context.AddAsync(country);
            await _context.SaveChangesAsync();
            return country;
        }

    }
}
