using AutoMapper;
using KaappaanPlus.Application.Contracts.Communication;
using KaappaanPlus.Application.Contracts.Persistence;
using KaappaanPlus.Application.Features.Alerts.Requests.Commands;
using KaappaanPlus.Domain.Entities;

using MediatR;
using Microsoft.AspNetCore.SignalR;
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
        private readonly IAlertNotifier _notifier;


        public CreateAlertHandler(
                 IAlertRepository alertRepo,
                 IAlertTypeRepository alertTypeRepo,
                 ICitizenRepository citizenRepo,
                 ITenantRepository tenantRepo,
                 IUserRepository userRepo,
                 IAlertResponderRepository responderRepo,
                 IAlertNotifier notifier)
        {
                _alertRepo = alertRepo;
                _alertTypeRepo = alertTypeRepo;
                _citizenRepo = citizenRepo;
                _tenantRepo = tenantRepo;
                _userRepo = userRepo;
                _responderRepo = responderRepo;
                _notifier = notifier;
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

            var alert = new Alert(
                citizen.Id,
                type.Id,
                dto.Description,
                dto.Latitude,
                dto.Longitude,
                type.Service
            )
            {
                Location = $"{dto.Latitude}, {dto.Longitude}"
            };

            await _alertRepo.AddAsync(alert);

            // 🔹 Determine which roles should receive the alert
            var dispatchOrder = new List<string>();
            switch (dto.AlertTypeName.Trim().ToLowerInvariant())
            {
                case "fire": dispatchOrder.AddRange(new[] { "Fire", "Ambulance", "Police" }); break;
                case "accident": dispatchOrder.AddRange(new[] { "Police", "Ambulance" }); break;
                case "crime":
                case "womensafety": dispatchOrder.Add("Police"); break;
                case "medical": dispatchOrder.Add("Ambulance"); break;
                default: dispatchOrder.Add("Police"); break;
            }

            // 🔹 Find responders by their roles in that tenant
            var responders = await _userRepo.GetRespondersByRolesAsync(tenant.Id, dispatchOrder, ct);

            foreach (var responder in responders)
            {
                await _responderRepo.AddAsync(new AlertResponder(alert.Id, responder.Id, "Auto-assigned"), ct);
            }

            // ✅ Create payload for SignalR
            var payload = new
            {
                AlertId = alert.Id,
                Type = dto.AlertTypeName,
                Description = dto.Description,
                Latitude = dto.Latitude,
                Longitude = dto.Longitude,
                Service = type.Service.ToString(),
                ReportedAt = DateTime.UtcNow
            };

            // ✅ Broadcast to all responder groups (Police, Fire, Ambulance)
            await _notifier.SendAlertAsync(payload, dispatchOrder.ToArray());

            return alert.Id;
        }

    }
}
