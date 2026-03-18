using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Books.Domain.Entities;
using Books.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Books.Application.Queries.Country
{
    public class GetCountryByIdHandler: IRequestHandler<GetCountryByIdQuery, CountryEntity>
    {
        private LibraryDbContext _context;

        public GetCountryByIdHandler(LibraryDbContext context)
        {
            this._context = context;
        }
        public async Task<CountryEntity?> Handle(GetCountryByIdQuery request, CancellationToken cancellationToken)
        {
            CountryEntity country = await _context.Countries.FirstOrDefaultAsync(c => c.Id == request.id);
            return country;
        }
    }
}
