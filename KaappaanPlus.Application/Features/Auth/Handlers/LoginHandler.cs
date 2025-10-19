using KaappaanPlus.Application.Contracts;
using KaappaanPlus.Application.Contracts.Identity;
using KaappaanPlus.Application.Features.Auth.DTOs;
using KaappaanPlus.Application.Features.Auth.Requests;
using KaappaanPlus.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaappaanPlus.Application.Features.Auth.Handlers
{
    public class LoginHandler : IRequestHandler<LoginCommand, LoginResponseDto>
    {
        private readonly IAuthService _authService;

        public LoginHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<LoginResponseDto> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            // ✅ No EF Core or password hasher here — clean architecture respected
            return await _authService.LoginAsync(request.LoginDto.Email, request.LoginDto.Password);
        }
    }
}

