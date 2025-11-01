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
        private readonly ILogger<GetAllTenantsHandler> _logger;

        public GetAllTenantsHandler(ITenantRepository tenantRepo, IMapper mapper, ILogger<GetAllTenantsHandler> logger)
        {
            _tenantRepo = tenantRepo;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<List<TenantDto>> Handle(GetAllTenantsQuery request, CancellationToken cancellationToken)
        {
            var tenants = await _tenantRepo.GetAllAsync(cancellationToken);

            if (!tenants.Any())
            {
                _logger.LogWarning("⚠️ No tenants found in the system.");
                return new List<TenantDto>();
            }

            _logger.LogInformation("✅ {Count} tenants retrieved successfully.", tenants.Count());
            return _mapper.Map<List<TenantDto>>(tenants);
        }
    }
}
