using AutoMapper;
using KaappaanPlus.Application.Contracts.Identity;
using KaappaanPlus.Application.Features.Auth.DTOs;
using KaappaanPlus.Application.Features.Auth.Requests;
using MediatR;
using Microsoft.Extensions.Logging;

namespace KaappaanPlus.Application.Features.Auth.Handlers
{
    public class LoginHandler : IRequestHandler<LoginCommand, LoginResponseDto>
    {
        private readonly IAuthService _authService;
       
        public LoginHandler(IAuthService authService, ILogger<LoginHandler> logger)
        {
            _authService = authService;
           
        }

        public async Task<LoginResponseDto> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _authService.LoginAsync(request.LoginDto.Email, request.LoginDto.Password);

                // 🔹 If user must change password
                if (result.Message.Contains("Password change required"))
                {
                    return new LoginResponseDto
                    {
                        Token = string.Empty,
                        Name = result.Name,
                        Role = result.Role,
                        Message = "Password change required before login"
                    };
                }

                return result;
            }
            catch (Exception ex)
            {

                // 🔹 General fallback exception
                throw new Exception($"Login failed due to an unexpected error: {ex.Message}");
            }
        }
    }
}
