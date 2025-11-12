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
    public class GetAlertsByTenantHandler : IRequestHandler<GetAlertsByTenantQuery, List<AlertDto>>
    {
        private readonly IAlertRepository _alertRepo;
        private readonly IMapper _mapper;
      

        public GetAlertsByTenantHandler(IAlertRepository alertRepo, IMapper mapper, ILogger<GetAlertsByTenantHandler> logger)
        {
            _alertRepo = alertRepo;
            _mapper = mapper;
          
        }

        public async Task<List<AlertDto>> Handle(GetAlertsByTenantQuery request, CancellationToken cancellationToken)
        {
            var alerts = await _alertRepo.GetAlertsByTenantAsync(request.TenantId);

            if (alerts == null || !alerts.Any())
            {
               
                return new List<AlertDto>();  // Return an empty list if no alerts are found.
            }

            

            // Mapping the alerts to AlertDto and returning the result
            return _mapper.Map<List<AlertDto>>(alerts);
        }

    }
}
