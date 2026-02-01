using KaappaanPlus.Application.Features.Alerts.DTOs;
using MediatR;
using System;
using System.Collections.Generic;

namespace KaappaanPlus.Application.Features.Alerts.Requests.Queries
{
    public class GetAlertsByResponderQuery : IRequest<List<AlertDto>>
    {
        public Guid ResponderId { get; set; }
    }
}
