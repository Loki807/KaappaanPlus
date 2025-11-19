using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaappaanPlus.Application.Features.Responders.Handlers.Commands
{
    public class UpdateResponderLocationCommand : IRequest<bool>
    {
        public Guid AlertId { get; set; }
        public Guid ResponderId { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }

}
