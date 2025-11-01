using KaappaanPlus.Application.Features.Tenants.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaappaanPlus.Application.Features.Tenants.Requests.Queries
{
    public class GetAllTenantsQuery : IRequest<List<TenantDto>> {
    
    }
}
