using KaappaanPlus.Application.Features.Alerts.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaappaanPlus.Application.Features.Alerts.Requests.Commands
{
    public class UpdateAlertCommand : IRequest<Guid>
    {
        public UpdateAlertDto UpdateAlertDto { get; set; } = default!;  // Ensure this property exists
    }
}
