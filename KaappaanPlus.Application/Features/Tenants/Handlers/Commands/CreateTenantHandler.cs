using AutoMapper;
using KaappaanPlus.Application.Contracts;
using KaappaanPlus.Application.Features.Tenants.Requests.Commands;
using KaappaanPlus.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaappaanPlus.Application.Features.Tenants.Handlers.Commands
{
    public class CreateTenantHandler : IRequestHandler<CreateTenantCommand, Guid>
    {
        private readonly IAppDbContext _dbContext;
        private readonly IMapper _mapper;

        public CreateTenantHandler(IAppDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<Guid> Handle(CreateTenantCommand request, CancellationToken cancellationToken)
        {
            // Map frontend DTO → real Tenant entity
            var tenant = _mapper.Map<Tenant>(request.TenantDto);

            // Example: If city = Colombo → shortPrefix = "COL"
            var shortPrefix = request.TenantDto.City?.Substring(0, 3).ToUpper() ?? "TEN";

            // Assign auto-generated code to Tenant.Code property
            tenant.GetType().GetProperty("Code")?.SetValue(tenant, $"{shortPrefix}_TENANT");

            // Add to DB using clean method (no EF here)
            await _dbContext.AddEntityAsync(tenant, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            // Return new Tenant ID
            return tenant.Id;
        }

    }
}
