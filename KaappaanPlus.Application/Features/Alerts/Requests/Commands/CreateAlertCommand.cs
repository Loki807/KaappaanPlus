using KaappaanPlus.Application.Features.Alerts.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaappaanPlus.Application.Features.Alerts.Requests.Commands
{
    public class CreateAlertCommand : IRequest<Guid>
    {
        public CreateAlertDto Alert { get; set; } = default!;
    }
}
