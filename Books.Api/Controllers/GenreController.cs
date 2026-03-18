using Books.Application.DTOs.BookDTOs;
using Books.Application.DTOs.GenreDTOs;
using Books.Application.Interfaces.Services;
using Books.Application.Service;
using Books.Domain.Entities;
using Books.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Books.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GenreController(IGenreService _GenreService): ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var genres = await _GenreService.GetAllGenresAsync(cancellationToken);
            return Ok(genres);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var genre = await _GenreService.GetGenreByIdAsync(id);
            return Ok(genre);
        }
        [Authorize(Roles ="Admin")]
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] GenreCreateDto request, CancellationToken cancellationToken)
        {
            int? genreId = await _GenreService.CreateGenreAsync(request, cancellationToken);
            return Ok(genreId);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            await _GenreService.Delete(id);
            return Ok();
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] GenreCreateDto update)
        {
            GenreReadDto newobj = await _GenreService.Update(id, update);
            return Ok(newobj);
        }
    }
}
