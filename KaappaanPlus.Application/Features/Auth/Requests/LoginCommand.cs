using KaappaanPlus.Application.Features.Auth.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaappaanPlus.Application.Features.Auth.Requests
{
    public class LoginCommand : IRequest<LoginResponseDto>
    {
        public LoginDto LoginDto { get; set; } = default!;
    }
}
