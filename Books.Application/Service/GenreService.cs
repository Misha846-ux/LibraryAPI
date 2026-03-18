using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Books.Application.DTOs.GenreDTOs;
using Books.Application.Interfaces.Repositories;
using Books.Application.Interfaces.Services;
using Books.Domain.Entities;

namespace Books.Application.Service
{
    public class GenreService : IGenreService
    {
        private readonly IGenreRepository _repository;
        private readonly IMapper _mapper;
        private readonly ICachingServices _cachingServices;
        private readonly string _cachingKey;

        public GenreService(IGenreRepository repository, IMapper mapper, ICachingServices cachingServices)
        {
            _repository = repository;
            _mapper = mapper;
            _cachingServices = cachingServices;

            _cachingKey = "Genres";
        }
        public async Task<int?> CreateGenreAsync(GenreCreateDto dto, CancellationToken cancellationToken)
        {
            try
            {
                await _cachingServices.RemoveAsync(_cachingKey, cancellationToken);
                GenreEntity genre = _mapper.Map<GenreEntity>(dto);
                return await _repository.AddGenreAsync(genre);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task Delete(int id)
        {
            await _repository.DeleteGenreAsync(id);
        }

        public async Task<ICollection<GenreReadDto>> GetAllGenresAsync(CancellationToken cancellationToken)
        {
            try
            {
                ICollection<GenreReadDto> data = await _cachingServices.GetAsync<ICollection<GenreReadDto>>(_cachingKey, cancellationToken);
                if(data == null)
                {
                    ICollection<GenreEntity> genres = await _repository.GetAllGenresAsync();
                    data = _mapper.Map<ICollection<GenreReadDto>>(genres);
                    await _cachingServices.SetAsync(_cachingKey, data, null, cancellationToken);
                }
                
                return data;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<GenreReadDto?> GetGenreByIdAsync(int id)
        {
            try
            {
                await _cachingServices.RemoveAsync(_cachingKey, new CancellationToken());
                GenreEntity genre = await _repository.GetGenreByIdAsync(id);
                return _mapper.Map<GenreReadDto>(genre);
            }
            catch(Exception ex)
            {
                return null;
            }
        }

        public async Task<GenreReadDto> Update(int id, GenreCreateDto genreCreateDto)
        {
            await _cachingServices.RemoveAsync(_cachingKey, new CancellationToken());
            GenreEntity genre = _mapper.Map<GenreEntity>(genreCreateDto);
            genre.Id = id;
            GenreEntity newGenre = await _repository.UpdateGenreAsync(genre);
            return _mapper.Map<GenreReadDto>(newGenre);
        }
    }
}
