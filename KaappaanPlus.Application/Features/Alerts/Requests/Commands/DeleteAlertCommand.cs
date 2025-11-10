using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaappaanPlus.Application.Features.Alerts.Requests.Commands
{
    public class DeleteAlertCommand : IRequest<Guid>
    {
        public Guid Id { get; set; }  // The ID of the alert to be deleted
    }
}
