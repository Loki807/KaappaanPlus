using KaappaanPlus.Application.Contracts.Persistence;
using KaappaanPlus.Application.Features.Alerts.Requests.Commands;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaappaanPlus.Application.Features.Alerts.Handlers.Commands
{
    public class UpdateAlertStatusHandler : IRequestHandler<UpdateAlertStatusCommand, Unit>
    {
        private readonly IAlertRepository _alertRepo;
        private readonly ILogger<UpdateAlertStatusHandler> _logger;

        public UpdateAlertStatusHandler(IAlertRepository alertRepo, ILogger<UpdateAlertStatusHandler> logger)
        {
            _alertRepo = alertRepo;
            _logger = logger;
        }

        public async Task<Unit> Handle(UpdateAlertStatusCommand request, CancellationToken cancellationToken)
        {
            var alert = await _alertRepo.GetByIdAsync(request.AlertId);
            if (alert == null)
                throw new Exception("Alert not found.");

            // ✅ Allowed status values: Pending / InProgress / Resolved
            var validStatuses = new[] { "Pending", "InProgress", "Resolved" };
            if (!validStatuses.Contains(request.Status))
                throw new Exception("Invalid status. Use: Pending, InProgress, or Resolved.");

            alert.UpdateStatus(request.Status);
            await _alertRepo.UpdateAsync(alert);

            _logger.LogInformation($"Alert {request.AlertId} updated to status: {request.Status}");
            return Unit.Value;
        }
    }
}
