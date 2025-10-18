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
            // ✅ Map CreateTenantDto -> Tenant entity
            CreateMap<CreateTenantDto, Tenant>()
                .ForMember(dest => dest.Code, opt => opt.Ignore());
            // Code is AUTO-GENERATED — do NOT map from DTO
        }
    }
}
