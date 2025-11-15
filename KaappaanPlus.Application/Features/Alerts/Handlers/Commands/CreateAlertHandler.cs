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
        private readonly IUserRepository _userRepo;
        private readonly IAlertResponderRepository _responderRepo;
        private readonly IAlertNotifier _notifier;

        public CreateAlertHandler(
            IAlertRepository alertRepo,
            IAlertTypeRepository alertTypeRepo,
            ICitizenRepository citizenRepo,
            IUserRepository userRepo,
            IAlertResponderRepository responderRepo,
            IAlertNotifier notifier)
        {
            _alertRepo = alertRepo;
            _alertTypeRepo = alertTypeRepo;
            _citizenRepo = citizenRepo;
            _userRepo = userRepo;
            _responderRepo = responderRepo;
            _notifier = notifier;
        }

        public async Task<Guid> Handle(CreateAlertCommand req, CancellationToken ct)
        {
            var dto = req.Alert;

            // 1️⃣ Validate Citizen
            var citizen = await _citizenRepo.GetByIdAsync(dto.CitizenId)
                           ?? throw new Exception("Citizen not found");

            // 2️⃣ Validate Alert Type
            var type = await _alertTypeRepo.GetByNameAsync(dto.AlertTypeName, ct)
                       ?? throw new Exception($"AlertType '{dto.AlertTypeName}' not found");

            // 3️⃣ Create Alert (no TenantId)
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

            // 4️⃣ Determine roles to notify
            var dispatchOrder = type.Service switch
            {
                ServiceType.Fire => new List<string> { "Fire", "Ambulance", "Police" },
                ServiceType.Ambulance => new List<string> { "Ambulance", "Police" },
                ServiceType.Police => new List<string> { "Police" },
                _ => new List<string> { "Police" }
            };

            // 5️⃣ Find responders across all tenants for these roles
            var responders = await _userRepo.GetRespondersByRolesAsync(Guid.Empty, dispatchOrder, ct);
            // 👆 Implement your repo to ignore tenantId if Guid.Empty

            // 6️⃣ Assign responders
            foreach (var responder in responders)
            {
                await _responderRepo.AddAsync(
                    new AlertResponder(alert.Id, responder.Id, "Auto-assigned global"), ct);
            }

            // 7️⃣ Build SignalR payload
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

            // 8️⃣ Broadcast live alert to all responder roles
            await _notifier.SendAlertAsync(payload, dispatchOrder.ToArray());

            return alert.Id;
        }
    }

}
