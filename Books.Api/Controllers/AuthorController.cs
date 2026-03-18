using Books.Application.DTOs.AuthorDTOs;
using Books.Application.DTOs.BookDTOs;
using Books.Application.DTOs.GenreDTOs;
using Books.Application.Interfaces.Services;
using Books.Domain.Entities;
using Books.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Books.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthorController(IAuthorService _authorService, IQueueService _queueService): ControllerBase
    {
        
        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var authors = await _authorService.GetAllAuthorsAsync(cancellationToken);
            return Ok(authors);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var author = await _authorService.GetAuthorByIdAsync(id);
            return Ok(author);
        }
        
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] AuthorCreateDto request, CancellationToken cancellationToken)
        {
            try
            {
                await _queueService.PublishAsync("Authors", request);
                int? authorId = await _authorService.CreateAuthorAsync(request, cancellationToken);
                return Created();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            await _authorService.Delete(id);
            return Ok();
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] AuthorCreateDto update)
        {
            AuthorReadDto newobj = await _authorService.Update(id, update);
            return Ok(newobj);
        }

    }
}
