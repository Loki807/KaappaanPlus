using AutoMapper;
using KaappaanPlus.Application.Contracts.Persistence;
using KaappaanPlus.Application.Features.Alerts.DTOs;
using KaappaanPlus.Application.Features.Alerts.Requests.Queries;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaappaanPlus.Application.Features.Alerts.Handlers.Queries
{
    public class GetAllAlertsHandler : IRequestHandler<GetAllAlertsQuery, List<AlertDto>>
    {
        private readonly IAlertRepository _alertRepo;
        private readonly IMapper _mapper;

        public GetAllAlertsHandler(IAlertRepository alertRepo, IMapper mapper)
        {
            _alertRepo = alertRepo;
            _mapper = mapper;
        }

        public async Task<List<AlertDto>> Handle(GetAllAlertsQuery request, CancellationToken cancellationToken)
        {
            var alerts = await _alertRepo.GetAllAsync(cancellationToken);
            return _mapper.Map<List<AlertDto>>(alerts);
        }
    }
}
