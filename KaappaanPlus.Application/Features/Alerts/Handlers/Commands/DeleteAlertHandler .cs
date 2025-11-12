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
    public class DeleteAlertHandler : IRequestHandler<DeleteAlertCommand, Guid>
    {
        private readonly IAlertRepository _alertRepo;
        

        public DeleteAlertHandler(IAlertRepository alertRepo, ILogger<DeleteAlertHandler> logger)
        {
            _alertRepo = alertRepo;
           
        }

        public async Task<Guid> Handle(DeleteAlertCommand request, CancellationToken cancellationToken)
        {
            // Step 1: Get the alert by ID
            var alert = await _alertRepo.GetByIdAsync(request.Id);
            if (alert == null)
            {
              
                throw new KeyNotFoundException($"Alert with ID {request.Id} not found.");
            }

            // Step 2: Delete the alert
            await _alertRepo.DeleteAsync(alert);

            // Step 3: Log the deletion
           

            // Step 4: Return the ID of the deleted alert
            return alert.Id;
        }
    }
}
