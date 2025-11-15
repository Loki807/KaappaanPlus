using AutoMapper;
using KaappaanPlus.Application.Contracts.Identity;
using KaappaanPlus.Application.Contracts.Persistence;
using KaappaanPlus.Application.Features.Auth.DTOs;
using KaappaanPlus.Application.Features.Auth.Requests;
using KaappaanPlus.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace KaappaanPlus.Application.Features.Auth.Handlers
{
    public class LoginHandler : IRequestHandler<LoginCommand, LoginResponseDto>
    {
        private readonly IUserRepository _userRepo;
        private readonly INotificationService _notification;

        public LoginHandler(IUserRepository userRepo, INotificationService notification)
        {
            _userRepo = userRepo;
            _notification = notification;
        }

        public async Task<LoginResponseDto> Handle(LoginCommand request, CancellationToken ct)
        {
            var dto = request.LoginDto;
            var user = await _userRepo.GetByEmailAsync(dto.Email, ct);

            if (user == null)
                throw new UnauthorizedAccessException("Invalid email or password");

            var hasher = new PasswordHasher<AppUser>();
            var verify = hasher.VerifyHashedPassword(user, user.PasswordHash!, dto.Password);
            if (verify == PasswordVerificationResult.Failed)
                throw new UnauthorizedAccessException("Invalid email or password");

            // 🔥 CITIZEN LOGIN → OTP
            if (user.Role == "Citizen")
            {
                var otp = new Random().Next(100000, 999999).ToString();

                user.EmailOtp = otp;
                user.OtpExpiryTime = DateTime.UtcNow.AddMinutes(5);
                user.IsEmailConfirmed = false;

                await _userRepo.UpdateAsync(user, ct);

                await _notification.SendEmailAsync(
                    user.Email,
                    "Kaappaan Login OTP",
                    $"<h2>Your OTP is <b>{otp}</b>. It expires in 5 minutes.</h2>"
                );

                return new LoginResponseDto
                {
                    IsEmailConfirmed = false,
                    Message = "OTP sent. Please verify OTP."
                };
            }

            // 🔥 ADMIN & TENANT ADMIN login flow
            if (user.MustChangePassword)
            {
                return new LoginResponseDto
                {
                    Token = "",
                    Name = user.Name,
                    Role = user.Role,
                    IsFirstLogin = true,
                    IsEmailConfirmed = true,
                    Message = "Password change required"
                };
            }

            // Normal admin login → return token
            return new LoginResponseDto
            {
                Token = "", // Token generated in AuthService
                Name = user.Name,
                Role = user.Role,
                IsEmailConfirmed = true,
                Message = "Login successful"
            };
        }
    }
}