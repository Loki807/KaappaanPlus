using AutoMapper;
using KaappaanPlus.Application.Features.Tenants.DTOs;
using KaappaanPlus.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaappaanPlus.Application.Features.Tenants.Mappings
{
    public class TenantMappingProfile : Profile
    {
        public TenantMappingProfile()
        {
            CreateMap<CreateTenantDto, Tenant>()
                     .ForMember(dest => dest.ServiceType, opt => opt.MapFrom(src => src.ServiceType));

            // 🧱 Create
            CreateMap<CreateTenantDto, Tenant>()
                .ForMember(dest => dest.Code, opt => opt.Ignore()); // generated automatically

            // 🔁 Update
            CreateMap<UpdateTenantDto, Tenant>()
                .ForMember(dest => dest.Code, opt => opt.Ignore())
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive));

            // 📤 Read (Entity → DTO)
            CreateMap<Tenant, TenantDto>();
        }
    }
}
