using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaappaanPlus.Application.Features.Citizens.Requests.Commands
{
    public class DeleteCitizenCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }   // Citizen Id
    }
}
