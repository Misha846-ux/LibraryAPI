using Books.Application.DTOs.AuthorDTOs;
using Books.Application.DTOs.BookDTOs;
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
    public class BookController(IBookService _bookService): ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var books = await _bookService.GetAllBooksAsync(cancellationToken);
            return Ok(books);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var book = await _bookService.GetBookByIdAsync(id);
            return Ok(book);
        }
        
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] BookCreateDto request, CancellationToken cancellationToken)
        {
            int? id = await _bookService.CreateBookAsync(request, cancellationToken);
            if(id != null)
            {
                return CreatedAtAction(nameof(GetById), new { id }, id);
            }
            else
            {
                return BadRequest();
            }
            
        }

        [HttpGet("filter")]
        public async Task<IActionResult> GetFiltered([FromQuery] int pageCount, int limit)
        {
            var books = await _bookService.GetBookFilteredAsync(pageCount, limit);
            return Ok(books);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            await _bookService.Delete(id);
            return Ok();
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] BookCreateDto update)
        {
            BookReadDto newobj = await _bookService.Update(id, update);
            return Ok(newobj);
        }
    }
}
