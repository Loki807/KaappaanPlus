using KaappaanPlus.Application.Features.Alerts.DTOs;
using MediatR;
using System;

namespace KaappaanPlus.Application.Features.Alerts.Requests.Queries
{
    public class GetAlertByIdQuery : IRequest<AlertDto>
    {
        public Guid Id { get; set; }
    }
}
