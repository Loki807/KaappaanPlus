using AutoMapper;
using KaappaanPlus.Application.Common.Exceptions;
using KaappaanPlus.Application.Contracts.Persistence;
using KaappaanPlus.Application.Features.Tenants.DTOs;
using KaappaanPlus.Application.Features.Tenants.Requests.Queries;
using KaappaanPlus.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaappaanPlus.Application.Features.Tenants.Handlers.Queries
{
    public class GetTenantByIdHandler : IRequestHandler<GetTenantByIdQuery, TenantDto>
    {
        private readonly ITenantRepository _tenantRepo;
        private readonly IMapper _mapper;
        private readonly ILogger<GetTenantByIdHandler> _logger;

        public GetTenantByIdHandler(ITenantRepository tenantRepo, IMapper mapper, ILogger<GetTenantByIdHandler> logger)
        {
            _tenantRepo = tenantRepo;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<TenantDto> Handle(GetTenantByIdQuery request, CancellationToken cancellationToken)
        {
            var tenant = await _tenantRepo.GetByIdAsync(request.Id, cancellationToken);

            if (tenant == null)
            {
                _logger.LogWarning("❌ Tenant with ID {Id} not found.", request.Id);
                throw new NotFoundException(nameof(tenant), request.Id);
            }

            _logger.LogInformation("✅ Tenant {Name} retrieved successfully.", tenant.Name);
            return _mapper.Map<TenantDto>(tenant);
        }
    }
}
