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
        private readonly IMapper _mapper;
        private readonly ILogger<CreateAlertHandler> _logger;

        public CreateAlertHandler(IAlertRepository alertRepo, IMapper mapper, ILogger<CreateAlertHandler> logger)
        {
            _alertRepo = alertRepo;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Guid> Handle(CreateAlertCommand request, CancellationToken cancellationToken)
        {
            var alert = _mapper.Map<Alert>(request.AlertDto);
            var id = await _alertRepo.AddAsync(alert, cancellationToken);

            _logger.LogInformation("🚨 New alert created: {Type} by {User}", alert.Type, alert.CreatedByUserId);
            return id;
        }
    }
}
