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
    public class UpdateAlertHandler : IRequestHandler<UpdateAlertCommand, Guid>
    {
        private readonly IAlertRepository _alertRepo;
        private readonly IAlertTypeRepository _alertTypeRepo;
       
        public UpdateAlertHandler(IAlertRepository alertRepo, IAlertTypeRepository alertTypeRepo, ILogger<UpdateAlertHandler> logger)
        {
            _alertRepo = alertRepo;
            _alertTypeRepo = alertTypeRepo;
           
        }

        public async Task<Guid> Handle(UpdateAlertCommand request, CancellationToken cancellationToken)
        {
            var dto = request.UpdateAlertDto; // This is the DTO you sent from the command

            // Step 1: Get the existing alert from the repository
            var alert = await _alertRepo.GetByIdAsync(dto.Id);
            if (alert == null)
            {
                throw new KeyNotFoundException("Alert not found.");
            }

            // Step 2: Optionally, update the AlertType if the AlertTypeName is provided
            if (!string.IsNullOrWhiteSpace(dto.AlertTypeName))
            {
                var alertType = await _alertTypeRepo.GetByNameAsync(dto.AlertTypeName, cancellationToken);
                if (alertType == null)
                {
                    throw new Exception($"AlertType '{dto.AlertTypeName}' not found.");
                }

                // Update the AlertType
                alert.AlertTypeId = alertType.Id;
                alert.AlertTypeRef = alertType; // Optionally store reference to the AlertType
            }

            // Step 3: Update other alert fields
            alert.UpdateStatus(dto.Status ?? "InProgress");  // Assuming UpdateStatus is implemented correctly
            alert.Description = dto.Description;
            alert.Latitude = dto.Latitude;
            alert.Longitude = dto.Longitude;

            // Step 4: Save the updated alert back to the repository
            await _alertRepo.UpdateAsync(alert);

            
            // Step 6: Return the updated alert ID
            return alert.Id;
        }

    }
}
