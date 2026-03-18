using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Books.Application.DTOs.BookDTOs;
using Books.Application.Interfaces.Repositories;
using Books.Application.Interfaces.Services;
using Books.Domain.Entities;

namespace Books.Application.Service
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _repository;
        private readonly IMapper _mapper;
        private readonly ICachingServices _cachingServices;
        private readonly string _cachingKey;

        public BookService(IBookRepository repository, IMapper mapper, ICachingServices cachingServices)
        {
            _repository = repository;
            _mapper = mapper;
            _cachingServices = cachingServices;

            _cachingKey = "Books";
        }

        public async Task<int?> CreateBookAsync(BookCreateDto dto, CancellationToken cancellationToken)
        {
            try
            {
                await _cachingServices.RemoveAsync(_cachingKey, cancellationToken);
                BookEntity book = _mapper.Map<BookEntity>(dto);
                return await _repository.AddBookAsync(book, dto.AuthersId);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task Delete(int id)
        {
            await _repository.DeleteBookAsync(id);
        }

        public async Task<ICollection<BookReadDto>> GetAllBooksAsync(CancellationToken cancellationToken)
        {
            try
            {
                ICollection<BookReadDto> data = await _cachingServices.GetAsync<ICollection<BookReadDto>>(_cachingKey, cancellationToken);
                if(data == null)
                {

                    ICollection<BookEntity> books = await _repository.GetAllBooksAsync();
                    data = _mapper.Map<ICollection<BookReadDto>>(books);
                    await _cachingServices.SetAsync(_cachingKey, data, null, cancellationToken);
                }
                return data;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<BookReadDto?> GetBookByIdAsync(int id)
        {
            try
            {
                BookEntity book = await _repository.GetBookByIdAsync(id);
                return _mapper.Map<BookReadDto>(book);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<ICollection<BookReadDto>> GetBookFilteredAsync(int pageCount, int limit)
        {
            try
            {
                await _cachingServices.RemoveAsync(_cachingKey, new CancellationToken());
                ICollection<BookEntity> books = await _repository.GetBookFilteredAsync(pageCount, limit);
                return _mapper.Map<ICollection<BookReadDto>>(books);
            }
            catch(Exception ex)
            {
                return null;
            }
        }

        public async Task<BookReadDto> Update(int id, BookCreateDto bookCreateDto)
        {
            await _cachingServices.RemoveAsync(_cachingKey, new CancellationToken());
            BookEntity book = _mapper.Map<BookEntity>(bookCreateDto);
            book.Id = id;
            BookEntity newBook = await _repository.UpdateBookAsync(book);
            return _mapper.Map<BookReadDto>(newBook);
        }
    }
}
