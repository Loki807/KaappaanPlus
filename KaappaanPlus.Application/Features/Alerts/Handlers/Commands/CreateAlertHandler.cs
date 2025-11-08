using AutoMapper;
using KaappaanPlus.Application.Contracts.Persistence;
using KaappaanPlus.Application.Features.Alerts.Requests.Commands;
using KaappaanPlus.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaappaanPlus.Application.Features.Alerts.Handlers.Commands
{
    public class CreateAlertHandler : IRequestHandler<CreateAlertCommand, Guid>
    {
        private readonly IAlertRepository _alertRepo;
        private readonly ICitizenRepository _citizenRepo;
        private readonly ITenantRepository _tenantRepo;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateAlertHandler> _logger;

        public CreateAlertHandler(
            IAlertRepository alertRepo,
            ICitizenRepository citizenRepo,
            ITenantRepository tenantRepo,
            IMapper mapper,
            ILogger<CreateAlertHandler> logger)
        {
            _alertRepo = alertRepo;
            _citizenRepo = citizenRepo;
            _tenantRepo = tenantRepo;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Guid> Handle(CreateAlertCommand request, CancellationToken cancellationToken)
        {
            var dto = request.AlertDto;

            // ✅ Validate Citizen
            var citizen = await _citizenRepo.GetByIdAsync(dto.CitizenId);
            if (citizen == null)
                throw new Exception("Citizen not found.");

            // ✅ Find matching Tenant (by City + AlertType)
            var tenant = await _tenantRepo.GetTenantByCityAndServiceAsync(dto.AlertType, citizen.AppUser.Email);
            if (tenant == null)
                throw new Exception($"No Tenant found for {dto.AlertType} service in your area.");

            // ✅ Create Alert
            var alert = new Alert(
                dto.CitizenId,
                tenant.Id,
                dto.AlertType,
                dto.Description,
                dto.Location
            );

            await _alertRepo.AddAsync(alert);
            _logger.LogInformation($"Alert created for Tenant {tenant.Name} by Citizen {citizen.Id}");

            return alert.Id;
        }
    }
}
