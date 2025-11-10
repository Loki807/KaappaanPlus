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
        private readonly IAlertTypeRepository _alertTypeRepo;
        private readonly ICitizenRepository _citizenRepo;
        private readonly ITenantRepository _tenantRepo;
        private readonly IUserRepository _userRepo;
        private readonly IAlertResponderRepository _responderRepo;
        private readonly ILogger<CreateAlertHandler> _logger;

        public CreateAlertHandler(
            IAlertRepository alertRepo,
            IAlertTypeRepository alertTypeRepo,
            ICitizenRepository citizenRepo,
            ITenantRepository tenantRepo,
            IUserRepository userRepo,
            IAlertResponderRepository responderRepo,
            ILogger<CreateAlertHandler> logger)
        {
            _alertRepo = alertRepo;
            _alertTypeRepo = alertTypeRepo;
            _citizenRepo = citizenRepo;
            _tenantRepo = tenantRepo;
            _userRepo = userRepo;
            _responderRepo = responderRepo;
            _logger = logger;
        }

        public async Task<Guid> Handle(CreateAlertCommand request, CancellationToken cancellationToken)
        {
            var dto = request.AlertDto;

            // ✅ 1. Validate citizen
            var citizen = await _citizenRepo.GetByIdAsync(dto.CitizenId);
            if (citizen == null)
                throw new Exception("Citizen not found.");

            // ✅ 2. Validate alert type
            var alertType = await _alertTypeRepo.GetByNameAsync(dto.AlertTypeName, cancellationToken);
            if (alertType == null)
                throw new Exception($"Invalid alert type: {dto.AlertTypeName}");

            // ✅ 3. Find matching tenant (by citizen city)
            var tenant = await _tenantRepo.GetByCityAsync(citizen.AppUser.Email); // adjust to actual city logic
            if (tenant == null)
                throw new Exception("No tenant found for this citizen’s area.");

            // ✅ 4. Create alert
            var alert = new Alert(
                dto.CitizenId,
                tenant.Id,
                alertType.Id,
                dto.Description,
                dto.Latitude,
                dto.Longitude
            );

            await _alertRepo.AddAsync(alert);

            // ✅ 5. Decide which responders should receive it
            var targetRoles = new List<string>();
            switch (dto.AlertTypeName.ToLower())
            {
                case "womensafety":
                case "crime":
                    targetRoles.Add("Police");
                    break;
                case "accident":
                    targetRoles.AddRange(new[] { "Police", "Ambulance" });
                    break;
                case "fire":
                    targetRoles.AddRange(new[] { "Fire", "Ambulance", "Police" });
                    break;
                case "medical":
                    targetRoles.AddRange(new[] { "Ambulance", "Police" });
                    break;
                default:
                    targetRoles.Add("Police");
                    break;
            }

            // ✅ 6. Find responders (Police/Fire/Ambulance)
            var responders = await _userRepo.GetRespondersByCityAndRolesAsync(tenant.City!, targetRoles, cancellationToken);

            foreach (var responder in responders)
            {
                await _responderRepo.AddAsync(new AlertResponder(alert.Id, responder.Id, "Auto-assigned"), cancellationToken);
            }

            _logger.LogInformation("✅ Alert created and assigned to {count} responders.", responders.Count);

            return alert.Id;
        }
    }
}
