using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Books.Application.DTOs.UserDTOs;
using Books.Domain.Entities;

namespace Books.Application.Mapping
{
    public class UserProfile: Profile
    {
        public UserProfile()
        {
            CreateMap<UserCreateDto, UserEntity>();

            CreateMap<UserEntity, UserReadDto>()
                .ForMember(dest => dest.Role, opt => opt.MapFrom(
                    src => src.Role.ToString()));

            CreateMap<UserEntity, UserLoginDto>();
        }
    }
}
