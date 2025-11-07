using KaappaanPlus.Application.Features.Alerts.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaappaanPlus.Application.Features.Alerts.Requests.Queries
{
    public class GetAlertsByCitizenQuery : IRequest<List<AlertDto>>
    {
        public Guid CitizenId { get; set; }
    }
}
