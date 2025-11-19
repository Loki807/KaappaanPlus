using KaappaanPlus.Application.Contracts.Persistence;
using KaappaanPlus.Application.Features.Responders.Request.Commands;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaappaanPlus.Application.Features.Responders.Handlers.Commands
{
    public class AcceptAlertHandler : IRequestHandler<AcceptAlertCommand, Guid>
    {
        private readonly IAlertResponderRepository _responderRepo;
        private readonly IUserRepository _userRepo;
        private readonly IAlertRepository _alertRepo;

        public AcceptAlertHandler(
            IAlertResponderRepository responderRepo,
            IUserRepository userRepo,
            IAlertRepository alertRepo
        )
        {
            _responderRepo = responderRepo;
            _userRepo = userRepo;
            _alertRepo = alertRepo;
        }

        public async Task<Guid> Handle(AcceptAlertCommand req, CancellationToken ct)
        {
            var responder = await _userRepo.GetByIdAsync(req.ResponderId)
                              ?? throw new Exception("Responder not found");

            var alert = await _alertRepo.GetByIdAsync(req.AlertId)
                          ?? throw new Exception("Alert not found");

            // Update responder status
            var mapping = await _responderRepo.GetByAlertAndResponderAsync(req.AlertId, req.ResponderId)
                           ?? throw new Exception("Responder is not assigned to this alert");

            mapping.AssignmentReason = "Responder accepted alert";
            mapping.SetUpdated(responder.Email);

            // Update alert status
            alert.UpdateStatus("InProgress");
            await _alertRepo.UpdateAsync(alert);

            return alert.Id;
        }
    }

}
