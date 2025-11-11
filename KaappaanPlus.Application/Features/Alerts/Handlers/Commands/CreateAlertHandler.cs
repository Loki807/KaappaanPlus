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

        public async Task<Guid> Handle(CreateAlertCommand req, CancellationToken ct)
        {
            var dto = req.Alert;

            var citizen = await _citizenRepo.GetByIdAsync(dto.CitizenId)
                          ?? throw new Exception("Citizen not found");

            var tenant = await _tenantRepo.GetByIdAsync(citizen.TenantId)
                         ?? throw new Exception("Tenant for citizen not found");

            var type = await _alertTypeRepo.GetByNameAsync(dto.AlertTypeName, ct)
                       ?? throw new Exception($"AlertType '{dto.AlertTypeName}' not found");

            var alert = new Alert(citizen.Id, tenant.Id, type.Id, dto.Description, dto.Latitude, dto.Longitude, type.Service);

            await _alertRepo.AddAsync(alert);

            // Dispatch logic based on AlertType
            var dispatchOrder = new List<string>();
            switch (dto.AlertTypeName.Trim().ToLowerInvariant())
            {
                case "accident": dispatchOrder.AddRange(new[] { "Police", "Ambulance" }); break;
                case "fire": dispatchOrder.AddRange(new[] { "Fire", "Ambulance", "Police" }); break;
                case "womensafety":
                case "crime": dispatchOrder.Add("Police"); break;
                case "medical": dispatchOrder.AddRange(new[] { "Ambulance", "Police" }); break;
                default: dispatchOrder.Add("Police"); break;
            }

            // Find responders (Police, Fire, Ambulance) based on dispatch logic
            var responders = await _userRepo.GetRespondersByRolesAsync(tenant.Id, dispatchOrder, ct);
            //foreach (var responder in responders)
            //{
            //    await _responderRepo.AddAsync(new AlertResponder(alert.Id, responder.Id, "Auto-assigned"), ct);
            //}

            _logger.LogInformation("Alert {AlertId} created for {Type} and assigned to {Count} responders.", alert.Id, type.Name, responders.Count());

            return alert.Id;
        }
    }
}
