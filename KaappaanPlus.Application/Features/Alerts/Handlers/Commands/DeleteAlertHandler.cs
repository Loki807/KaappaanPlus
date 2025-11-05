using KaappaanPlus.Application.Contracts.Persistence;
using KaappaanPlus.Application.Features.Alerts.Requests.Commands;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaappaanPlus.Application.Features.Alerts.Handlers.Commands
{
    public class DeleteAlertHandler : IRequestHandler<DeleteAlertCommand>
    {
        private readonly IAlertRepository _alertRepo;

        public DeleteAlertHandler(IAlertRepository alertRepo) => _alertRepo = alertRepo;

        public async Task<Unit> Handle(DeleteAlertCommand request, CancellationToken cancellationToken)
        {
            await _alertRepo.DeleteAsync(request.Id, cancellationToken);
            return Unit.Value;
        }
    }
}
