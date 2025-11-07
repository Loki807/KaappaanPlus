using KaappaanPlus.Application.Features.Alerts.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaappaanPlus.Application.Features.Alerts.Requests.Queries
{
    public class GetAlertsByTenantQuery : IRequest<List<AlertDto>>
    {
        public Guid TenantId { get; set; }
    }
}
