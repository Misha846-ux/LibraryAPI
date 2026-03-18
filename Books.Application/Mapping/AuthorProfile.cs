using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Books.Application.DTOs.AuthorDTOs;
using Books.Domain.Entities;

namespace Books.Application.Mapping
{
    public class AuthorProfile: Profile
    {
        public AuthorProfile()
        {
            CreateMap<AuthorCreateDto, AuthorEntity>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Books, opt => opt.Ignore());
            CreateMap<AuthorEntity, AuthorReadDto>()
                .ForMember(dest => dest.BooksId, opt => opt.MapFrom(
                    src => src.Books.Select(x => x.Id)));
        }
    }
}
