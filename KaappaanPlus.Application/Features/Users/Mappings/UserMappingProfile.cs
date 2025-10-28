using AutoMapper;
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
                .ConstructUsing(dto => new AppUser(
                    dto.TenantId,
                    dto.Name,
                    dto.Email,
                    dto.Phone,
                    BCrypt.Net.BCrypt.HashPassword(dto.Password), // secure
                    dto.Role
                ));
        }
    }
}
