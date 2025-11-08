using AutoMapper;
using KaappaanPlus.Application.Features.Citizens.DTOs;
using KaappaanPlus.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaappaanPlus.Application.Features.Citizens.Mappings
{
    public class CitizenProfile : Profile
    {
        public CitizenProfile()
        {
            CreateMap<Citizen, CitizenDto>()
                  .ForMember(dest => dest.AppUserId, opt => opt.MapFrom(src => src.AppUserId))
                  .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.AppUser.Name))
                  .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.AppUser.Email))
                  .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.AppUser.Phone))
                  .ForMember(dest => dest.NIC, opt => opt.MapFrom(src => src.NIC))
                  .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
                  .ForMember(dest => dest.EmergencyContact, opt => opt.MapFrom(src => src.EmergencyContact));

            // ✅ Login
            CreateMap<Citizen, CitizenLoginResponseDto>();

            // ✅ Update
            CreateMap<UpdateCitizenDto, Citizen>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            // ✅ For viewing
            CreateMap<Citizen, CitizenRegisterDto>();

            // 🔹 View (Entity → DTO)
            CreateMap<Citizen, CitizenDto>();

            // 🔹 Optional reverse mapping (for tests / admin forms)
            CreateMap<CitizenDto, Citizen>();
        }
    }
}
