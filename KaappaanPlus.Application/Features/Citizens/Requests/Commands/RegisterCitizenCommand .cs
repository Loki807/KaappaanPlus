using KaappaanPlus.Application.Features.Citizens.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaappaanPlus.Application.Features.Citizens.Requests.Commands
{
    public class RegisterCitizenCommand : IRequest<Guid>
    {
        public CitizenRegisterDto Citizen { get; set; } = default!;
    }
}
