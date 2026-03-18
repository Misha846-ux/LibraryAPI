using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Books.Application.DTOs.BookDTOs;
using Books.Domain.Entities;

namespace Books.Application.Mapping
{
    public class BookProfile: Profile
    {
        public BookProfile()
        {
            CreateMap<BookCreateDto, BookEntity>()
                .ForMember(dest => dest.Authors, opt => opt.Ignore());
            CreateMap<BookEntity, BookReadDto>()
                .ForMember(dest => dest.AuthorsId, opt => opt.MapFrom(
                    src => src.Authors.Select(a=> a.Id)));
        }
    }
}
