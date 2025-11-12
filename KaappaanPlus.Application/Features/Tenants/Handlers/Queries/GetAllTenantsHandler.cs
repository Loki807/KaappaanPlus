using AutoMapper;
using KaappaanPlus.Application.Contracts.Persistence;
using KaappaanPlus.Application.Features.Tenants.DTOs;
using KaappaanPlus.Application.Features.Tenants.Requests.Queries;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaappaanPlus.Application.Features.Tenants.Handlers.Queries
{
    public class GetAllTenantsHandler : IRequestHandler<GetAllTenantsQuery, List<TenantDto>>
    {
        private readonly ITenantRepository _tenantRepo;
        private readonly IMapper _mapper;

        public GetAllTenantsHandler(ITenantRepository tenantRepo, IMapper mapper)
        {
            _tenantRepo = tenantRepo;
            _mapper = mapper;
        }

        public async Task<List<TenantDto>> Handle(GetAllTenantsQuery request, CancellationToken cancellationToken)
        {
            var tenants = await _tenantRepo.GetAllAsync(cancellationToken);

            if (tenants == null || !tenants.Any())
            {
                // ⚠️ If no tenants exist, return empty list or throw message
                throw new Exception("No tenants found in the system.");
            }

            // ✅ Successfully retrieved
            var tenantList = _mapper.Map<List<TenantDto>>(tenants);
            if (tenantList == null || tenantList.Count == 0)
                throw new Exception("Failed to map tenants data.");

            return tenantList;
        }
    }
}
