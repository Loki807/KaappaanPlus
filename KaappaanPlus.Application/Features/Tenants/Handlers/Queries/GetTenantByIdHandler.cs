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

        public GetTenantByIdHandler(ITenantRepository tenantRepo, IMapper mapper)
        {
            _tenantRepo = tenantRepo;
            _mapper = mapper;
        }

        public async Task<TenantDto> Handle(GetTenantByIdQuery request, CancellationToken cancellationToken)
        {
            // 🔹 Step 1: Retrieve tenant by ID from repository
            var tenant = await _tenantRepo.GetByIdAsync(request.Id, cancellationToken);

            // 🔹 Step 2: If no tenant found, throw domain-friendly exception
            if (tenant == null)
            {
                throw new NotFoundException("Tenant", request.Id);
            }

            // 🔹 Step 3: Map entity → DTO safely
            var mappedTenant = _mapper.Map<TenantDto>(tenant);

            // 🔹 Step 4: Safety check (optional)
            if (mappedTenant == null)
                throw new Exception("Failed to map tenant details.");

            // 🔹 Step 5: Return the final DTO
            return mappedTenant;
        }
    }
}
