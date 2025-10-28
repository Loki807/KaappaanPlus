using KaappaanPlus.Application.Contracts.Identity;
using KaappaanPlus.Application.Features.Auth.Requests;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaappaanPlus.Application.Features.Auth.Handlers
{
    public class ChangePasswordHandler : IRequestHandler<ChangePasswordCommand, bool>
    {
        private readonly IAuthService _authService;

        public ChangePasswordHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<bool> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            var dto = request.ChangePasswordDto;

            await _authService.ChangePasswordAsync(dto.Email, dto.OldPassword, dto.NewPassword);
            return true;
        }
    }
}
