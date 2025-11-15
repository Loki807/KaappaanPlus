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
    public class GetAlertRespondersHandler : IRequestHandler<GetAlertRespondersQuery, List<AlertResponderDto>>
    {
        private readonly IAlertResponderRepository _responderRepo;
        private readonly IMapper _mapper;

        public GetAlertRespondersHandler(IAlertResponderRepository responderRepo, IMapper mapper)
        {
            _responderRepo = responderRepo;
            _mapper = mapper;
        }

        public async Task<List<AlertResponderDto>> Handle(GetAlertRespondersQuery request, CancellationToken cancellationToken)
        {
            var responders = await _responderRepo.GetByAlertIdAsync(request.AlertId, cancellationToken);

            return responders.Select(r => new AlertResponderDto
            {
                ResponderId = r.ResponderId,
                ResponderName = r.Responder.Name,         // ✔ Correct property
                Role = r.Responder.Role,                  // ✔ Role from AppUser
                AssignmentReason = r.AssignmentReason
            }).ToList();
        }

    }
}
