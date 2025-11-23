using KaappaanPlus.Application.Contracts.Identity;
using KaappaanPlus.Application.Contracts.Persistence;
using KaappaanPlus.Application.Features.Auth.DTOs;
using KaappaanPlus.Application.Features.Auth.Requests;
using KaappaanPlus.Application.Features.Citizens.DTOs;
using KaappaanPlus.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace KaappaanPlus.Application.Features.Auth.Handlers
{
    public class LoginHandler : IRequestHandler<LoginCommand, object>
    {
        private readonly IUserRepository _userRepo;
        private readonly IAuthService _authService;
        private readonly INotificationService _notification;

        public LoginHandler(
            IUserRepository userRepo,
            IAuthService authService,
            INotificationService notification)
        {
            _userRepo = userRepo;
            _authService = authService;
            _notification = notification;
        }

        public async Task<object> Handle(LoginCommand request, CancellationToken ct)
        {
            var dto = request.LoginDto;

            // 🔍 Find user
            var user = await _userRepo.GetByEmailAsync(dto.Email, ct);
            if (user == null)
                throw new UnauthorizedAccessException("Invalid email or password");

            // 🔐 Verify Password
            var hasher = new PasswordHasher<AppUser>();
            var verify = hasher.VerifyHashedPassword(user, user.PasswordHash!, dto.Password);
            if (verify == PasswordVerificationResult.Failed)
                throw new UnauthorizedAccessException("Invalid email or password");

            string role = user.Role;

            // ⭐ CITIZEN LOGIN → OTP FLOW
            if (role == "Citizen")
            {
                var otp = new Random().Next(100000, 999999).ToString();

                user.EmailOtp = otp;
                user.OtpExpiryTime = DateTime.UtcNow.AddMinutes(5);
                user.IsEmailConfirmed = false;

                await _userRepo.UpdateAsync(user, ct);

                await _notification.SendEmailAsync(
                    user.Email,
                    "Kaappaan Login OTP",
                    $"<h2>Your OTP is <b>{otp}</b></h2><p>Expires in 5 mins.</p>"
                );

                return new CitizenLoginResponseDto
                {
                    Token = "",
                    FullName = user.Name,
                    Message = "OTP sent. Please verify.",
                    CitizenId = user.Id,
                    IsEmailConfirmed = false
                };
            }

            // ⭐ ADMIN (TENANT ADMIN / SUPER ADMIN)
            if (user.MustChangePassword)
            {
                return new LoginResponseDto
                {
                    Token = "",
                    Name = user.Name,
                    Role = user.Role,
                    Message = "Password change required",
                    IsFirstLogin = true,
                    IsEmailConfirmed = true
                };
            }

            // ⭐ NORMAL ADMIN LOGIN (JWT)
            return await _authService.LoginAsync(dto.Email, dto.Password);
        }
    }
}
