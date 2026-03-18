using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Books.Domain.Entities;
using MediatR;

namespace Books.Application.Queries.Country
{
    public record GetCountryByIdQuery(int id): IRequest<CountryEntity>;
}
