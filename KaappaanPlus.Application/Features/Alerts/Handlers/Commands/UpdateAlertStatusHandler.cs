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
    public class UpdateAlertStatusHandler : IRequestHandler<UpdateAlertStatusCommand>
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
            var alert = await _alertRepo.GetByIdAsync(request.Id, cancellationToken);
            if (alert == null) throw new KeyNotFoundException($"Alert {request.Id} not found.");

            alert.UpdateStatus(request.Status);
            await _alertRepo.UpdateAsync(alert, cancellationToken);

            _logger.LogInformation("✅ Alert {Id} status updated to {Status}", request.Id, request.Status);
            return Unit.Value;
        }
    }
}
