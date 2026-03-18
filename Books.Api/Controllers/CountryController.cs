using Books.Application.Queries.Country;
using Books.Infrastructure.Commands.Country;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Books.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CountryController(IMediator _mediator): ControllerBase
    {
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCountryById([FromRoute] int id)
        {
            return Ok( await _mediator.Send(new GetCountryByIdQuery(id)));
        }

        [HttpPost]
        public async Task<IActionResult> AddCountry([FromBody] string name)
        {
            return Ok(await _mediator.Send(new CreateCountryCommand(name)));
        }

    }
}
