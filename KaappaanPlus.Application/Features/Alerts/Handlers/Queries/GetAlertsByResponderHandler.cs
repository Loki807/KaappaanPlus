using AutoMapper;
using KaappaanPlus.Application.Contracts.Persistence;
using KaappaanPlus.Application.Features.Alerts.DTOs;
using KaappaanPlus.Application.Features.Alerts.Requests.Queries;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace KaappaanPlus.Application.Features.Alerts.Handlers.Queries
{
    public class GetAlertsByResponderHandler : IRequestHandler<GetAlertsByResponderQuery, List<AlertDto>>
    {
        private readonly IAlertResponderRepository _alertResponderRepo;
        private readonly IMapper _mapper;

        public GetAlertsByResponderHandler(IAlertResponderRepository alertResponderRepo, IMapper mapper)
        {
            _alertResponderRepo = alertResponderRepo;
            _mapper = mapper;
        }

        public async Task<List<AlertDto>> Handle(GetAlertsByResponderQuery request, CancellationToken cancellationToken)
        {
            var mappings = await _alertResponderRepo.GetByResponderIdAsync(request.ResponderId, cancellationToken);
            
            if (mappings == null || !mappings.Any())
            {
                return new List<AlertDto>();
            }

            // Extract the alerts from the AlertResponder mappings
            var alerts = mappings.Select(m => m.Alert).ToList();

            return _mapper.Map<List<AlertDto>>(alerts);
        }
    }
}
