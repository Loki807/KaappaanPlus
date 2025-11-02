using AutoMapper;
using KaappaanPlus.Application.Features.Users.DTOs;
using KaappaanPlus.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaappaanPlus.Application.Features.Users.Mappings
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<CreateUserDto, AppUser>()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore());

            CreateMap<UpdateUserDto, AppUser>()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore());

            CreateMap<AppUser, UserDto>();
        }
    }
}
