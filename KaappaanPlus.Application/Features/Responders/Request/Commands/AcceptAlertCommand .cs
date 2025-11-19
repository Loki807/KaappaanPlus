using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaappaanPlus.Application.Features.Responders.Request.Commands
{
    public class AcceptAlertCommand : IRequest<Guid>
    {
        public Guid AlertId { get; set; }
        public Guid ResponderId { get; set; }   // from JWT
    }

}

