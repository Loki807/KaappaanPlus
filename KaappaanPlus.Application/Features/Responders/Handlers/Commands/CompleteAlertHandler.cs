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
    public class CompleteAlertHandler : IRequestHandler<CompleteAlertCommand, Guid>
    {
        private readonly IAlertRepository _alertRepo;

        public CompleteAlertHandler(IAlertRepository alertRepo)
        {
            _alertRepo = alertRepo;
        }

        public async Task<Guid> Handle(CompleteAlertCommand req, CancellationToken ct)
        {
            var alert = await _alertRepo.GetByIdAsync(req.AlertId)
                          ?? throw new Exception("Alert not found");

            alert.UpdateStatus("Completed");
            await _alertRepo.UpdateAsync(alert);

            return alert.Id;
        }
    }

}
