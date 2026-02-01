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
       

        public UpdateTenantHandler(ITenantRepository tenantRepo, IMapper mapper, ILogger<UpdateTenantHandler> logger)
        {
            _tenantRepo = tenantRepo;
            _mapper = mapper;
         
        }

        public async Task<Unit> Handle(UpdateTenantCommand request, CancellationToken cancellationToken)
        {
            var existingTenant = await _tenantRepo.GetByIdAsync(request.TenantDto.Id, cancellationToken);

            if (existingTenant == null)
            {
                throw new NotFoundException(nameof(existingTenant), request.TenantDto.Id);
            }

            // Update Basic Info
            existingTenant.UpdateInfo(
                request.TenantDto.Name,
                request.TenantDto.Code,
                request.TenantDto.Email,
                request.TenantDto.ServiceType,
                request.TenantDto.ContactNumber,
                request.TenantDto.LogoUrl
            );

            // Update Address
            existingTenant.UpdateAddress(
                request.TenantDto.AddressLine1,
                request.TenantDto.AddressLine2,
                request.TenantDto.City,
                request.TenantDto.StateOrDistrict,
                request.TenantDto.PostalCode
            );

            if (request.TenantDto.IsActive) existingTenant.Activate();
            else existingTenant.Deactivate();

            await _tenantRepo.UpdateAsync(existingTenant, cancellationToken);

            
            return Unit.Value;
        }
    }
}
