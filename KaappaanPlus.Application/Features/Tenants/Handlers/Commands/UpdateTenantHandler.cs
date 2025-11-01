using AutoMapper;
using KaappaanPlus.Application.Common.Exceptions;
using KaappaanPlus.Application.Contracts.Persistence;
using KaappaanPlus.Application.Features.Tenants.Requests.Commands;
using KaappaanPlus.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaappaanPlus.Application.Features.Tenants.Handlers.Commands
{
    public class UpdateTenantHandler : IRequestHandler<UpdateTenantCommand, Unit>
    {
        private readonly ITenantRepository _tenantRepo;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateTenantHandler> _logger;

        public UpdateTenantHandler(ITenantRepository tenantRepo, IMapper mapper, ILogger<UpdateTenantHandler> logger)
        {
            _tenantRepo = tenantRepo;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Unit> Handle(UpdateTenantCommand request, CancellationToken cancellationToken)
        {
            var existingTenant = await _tenantRepo.GetByIdAsync(request.TenantDto.Id, cancellationToken);

            if (existingTenant == null)
            {
                _logger.LogWarning("❌ Tenant with ID {Id} not found.", request.TenantDto.Id);
                throw new NotFoundException(nameof(existingTenant), request.TenantDto.Id);
            }

            _mapper.Map(request.TenantDto, existingTenant);
            await _tenantRepo.UpdateAsync(existingTenant, cancellationToken);

            _logger.LogInformation("✅ Tenant {Name} updated successfully.", existingTenant.Name);
            return Unit.Value;
        }
    }
}
