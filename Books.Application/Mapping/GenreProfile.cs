using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Books.Application.DTOs.GenreDTOs;
using Books.Domain.Entities;

namespace Books.Application.Mapping
{
    public class GenreProfile: Profile
    {
        public GenreProfile()
        {
            CreateMap<GenreCreateDto, GenreEntity>();
            CreateMap<GenreEntity, GenreReadDto>()
                .ForMember(dest => dest.Books, opt => opt.MapFrom(
                    src => src.Books.Select(x => x.Id)));
        }
    }
}
