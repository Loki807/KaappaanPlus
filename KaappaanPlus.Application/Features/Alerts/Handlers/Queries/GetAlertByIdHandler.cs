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
    public class GetAlertByIdHandler : IRequestHandler<GetAlertByIdQuery, AlertDto>
    {
        private readonly IAlertRepository _alertRepo;
        private readonly IMapper _mapper;

        public GetAlertByIdHandler(IAlertRepository alertRepo, IMapper mapper)
        {
            _alertRepo = alertRepo;
            _mapper = mapper;
        }

        public async Task<AlertDto> Handle(GetAlertByIdQuery request, CancellationToken cancellationToken)
        {
            var alert = await _alertRepo.GetByIdAsync(request.Id, cancellationToken);
            if (alert == null) throw new KeyNotFoundException($"Alert with ID {request.Id} not found.");
            return _mapper.Map<AlertDto>(alert);
        }
    }
}
