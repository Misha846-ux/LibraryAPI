using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Books.Application.DTOs.AuthorDTOs;
using Books.Application.Interfaces.Repositories;
using Books.Application.Interfaces.Services;
using Books.Domain.Entities;

namespace Books.Application.Service
{
    public class AuthorService: IAuthorService
    {
        private readonly IAuthorRepository _repository;
        private readonly IMapper _mapper;
        private readonly ICachingServices _cachingServices;
        private readonly string _cachingKey;

        public AuthorService(IAuthorRepository repository, IMapper mapper, ICachingServices cachingServices)
        {
            _repository = repository;
            _mapper = mapper;
            _cachingServices = cachingServices;

            _cachingKey = "Authors";
        }
        public async Task<int?> CreateAuthorAsync(AuthorCreateDto dto, CancellationToken cancellationToken)
        {
            try
            {
                await _cachingServices.RemoveAsync(_cachingKey, cancellationToken);
                AuthorEntity author = _mapper.Map<AuthorEntity>(dto);
                return await _repository.AddAuthorAsync(author, cancellationToken);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task Delete(int id)
        {
            await _repository.DeleteAuthorAsync(id, new CancellationToken());
        }

        public async Task<ICollection<AuthorReadDto>> GetAllAuthorsAsync(CancellationToken cancellationToken)
        {
            try
            {
                ICollection<AuthorReadDto> data = await _cachingServices.GetAsync<ICollection<AuthorReadDto>>(_cachingKey, cancellationToken);
                if(data == null)
                {
                    ICollection<AuthorEntity> authors = await _repository.GetAllAuthorsAsync(cancellationToken);
                    data = _mapper.Map<ICollection<AuthorReadDto>>(authors);
                    await _cachingServices.SetAsync(_cachingKey, data, TimeSpan.FromMinutes(15), cancellationToken);

                }

                return data;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<AuthorReadDto?> GetAuthorByIdAsync(int id)
        {
            try
            {
                await _cachingServices.RemoveAsync(_cachingKey, new CancellationToken());
                AuthorEntity author = await _repository.GetAuthorByIdAsync(id);
                return _mapper.Map<AuthorReadDto>(author);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<AuthorReadDto> Update(int id, AuthorCreateDto authorCreateDto)
        {
            await _cachingServices.RemoveAsync(_cachingKey, new CancellationToken());
            AuthorEntity author = _mapper.Map<AuthorEntity>(authorCreateDto);
            author.Id = id;
            AuthorEntity newAuthor = await _repository.UpdateAuthorAsync(author, new CancellationToken());
            return _mapper.Map<AuthorReadDto>(newAuthor);
        }
    }
}
