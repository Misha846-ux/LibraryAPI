using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Books.Domain.Entities;
using MediatR;

namespace Books.Infrastructure.Commands.Country
{
    public record CreateCountryCommand(string name):IRequest<CountryEntity>
    {
    }
}
