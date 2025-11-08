using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaappaanPlus.Application.Features.Alerts.Requests.Commands
{
    public class UpdateAlertStatusCommand : IRequest<Unit>
    {
        public Guid AlertId { get; set; }
        public string Status { get; set; } = default!;
    }
}
