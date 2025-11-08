using AutoMapper;
using KaappaanPlus.Application.Contracts.Persistence;
using KaappaanPlus.Application.Features.Alerts.DTOs;
using KaappaanPlus.Application.Features.Alerts.Requests.Queries;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaappaanPlus.Application.Features.Alerts.Handlers.Queries
{
    public class GetAlertsByCitizenHandler : IRequestHandler<GetAlertsByCitizenQuery, List<AlertDto>>
    {
        private readonly IAlertRepository _alertRepo;
        private readonly IMapper _mapper;
        private readonly ILogger<GetAlertsByCitizenHandler> _logger;

        public GetAlertsByCitizenHandler(IAlertRepository alertRepo, IMapper mapper, ILogger<GetAlertsByCitizenHandler> logger)
        {
            _alertRepo = alertRepo;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<List<AlertDto>> Handle(GetAlertsByCitizenQuery request, CancellationToken cancellationToken)
        {
            var alerts = await _alertRepo.GetAlertsByCitizenAsync(request.CitizenId);
            _logger.LogInformation($"Fetched {alerts.Count} alerts for Citizen: {request.CitizenId}");

            return _mapper.Map<List<AlertDto>>(alerts);
        }
    }
}
