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
            // MAIN MAPPING (Entity → DTO)
            CreateMap<Citizen, CitizenDto>()
                .ForMember(dest => dest.AppUserId, opt => opt.MapFrom(src => src.AppUserId))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.AppUser.Name))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.AppUser.Email))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.AppUser.Phone))
                .ForMember(dest => dest.NIC, opt => opt.MapFrom(src => src.NIC))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
                .ForMember(dest => dest.EmergencyContact, opt => opt.MapFrom(src => src.EmergencyContact));

            // LOGIN RESPONSE DTO
            CreateMap<Citizen, CitizenLoginResponseDto>();

            // UPDATE DTO → ENTITY
            CreateMap<UpdateCitizenDto, Citizen>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
        }
    }
}

