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
            // ✅ CreateUserDto → AppUser
            CreateMap<CreateUserDto, AppUser>()
                .ConstructUsing(dto =>
                    new AppUser(
                        dto.TenantId,
                        dto.Name,
                        dto.Email,
                        dto.Phone,
                        BCrypt.Net.BCrypt.HashPassword(dto.Password),
                        dto.Role
                    )
                );

            // ✅ AppUser → UserResponseDto
            CreateMap<AppUser, UserResponseDto>();

            // ✅ UpdateUserDto → AppUser
            CreateMap<UpdateUserDto, AppUser>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore());
        }
    }
}
