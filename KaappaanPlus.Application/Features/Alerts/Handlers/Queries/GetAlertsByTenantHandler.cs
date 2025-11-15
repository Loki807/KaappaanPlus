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
    public class GetAlertsByTenantIdHandler : IRequestHandler<GetAlertsByTenantIdQuery, List<AlertDto>>
    {
        private readonly IAlertRepository _alertRepo;
        private readonly IMapper _mapper;

        public GetAlertsByTenantIdHandler(IAlertRepository alertRepo, IMapper mapper)
        {
            _alertRepo = alertRepo;
            _mapper = mapper;
        }

        public async Task<List<AlertDto>> Handle(GetAlertsByTenantIdQuery request, CancellationToken ct)
        {
            var alerts = await _alertRepo.GetAlertsByTenantIdAsync(request.TenantId, ct);
            return _mapper.Map<List<AlertDto>>(alerts);
        }

    }
}