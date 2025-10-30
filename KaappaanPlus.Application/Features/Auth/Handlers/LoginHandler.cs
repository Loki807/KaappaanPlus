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
        private readonly ILogger<LoginHandler> _logger;

        public LoginHandler(IAuthService authService, ILogger<LoginHandler> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        public async Task<LoginResponseDto> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _authService.LoginAsync(request.LoginDto.Email, request.LoginDto.Password);

                // 🔹 If user must change password
                if (result.Message.Contains("Password change required"))
                {
                    _logger.LogWarning("Password change required for {Email}", request.LoginDto.Email);
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
                _logger.LogError(ex, "Login failed for {Email}", request.LoginDto.Email);
                throw;
            }
        }
    }
}
