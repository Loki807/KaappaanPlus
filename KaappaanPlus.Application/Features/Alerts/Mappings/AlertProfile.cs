using AutoMapper;
using KaappaanPlus.Application.Features.Alerts.DTOs;
using KaappaanPlus.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaappaanPlus.Application.Features.Alerts.Mappings
{
    public class AlertProfile : Profile
    {
        public AlertProfile()
        {
            CreateMap<Alert, AlertDto>();
                
        }
    }
}
