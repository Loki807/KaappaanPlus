using KaappaanPlus.Application.Contracts.Identity;
using KaappaanPlus.Application.Contracts.Persistence;
using KaappaanPlus.Application.Features.Auth.DTOs;
using KaappaanPlus.Application.Features.Auth.Requests;
using KaappaanPlus.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace KaappaanPlus.Application.Features.Auth.Handlers
{
    public class LoginHandler : IRequestHandler<LoginCommand, LoginResponseDto>
    {
        private readonly IUserRepository _userRepo;
        private readonly IAuthService _authService;
        private readonly INotificationService _notification;
        private readonly ICitizenRepository _citizenRepo;     // ⭐ ADDED

        public LoginHandler(
            IUserRepository userRepo,
            IAuthService authService,
            INotificationService notification,
            ICitizenRepository citizenRepo)                  // ⭐ ADDED
        {
            _userRepo = userRepo;
            _authService = authService;
            _notification = notification;
            _citizenRepo = citizenRepo;                     // ⭐ ADDED
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

            string role = user.Role;

            // ⭐ ONLY CITIZEN → OTP
            if (role == "Citizen")
            {
                // ⭐ Get real citizen
                var citizen = await _citizenRepo.GetByUserIdAsync(user.Id);
                if (citizen == null)
                    throw new Exception("Citizen record not found for this user");

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

                return new LoginResponseDto
                {
                    IsEmailConfirmed = false,
                    Message = "OTP sent. Please verify.",
                    Name = user.Name,
                    Role = user.Role,
                    Token = "",
                    CitizenId = citizen.Id   // ⭐ FIXED
                };
            }

            if (user.MustChangePassword)
            {
                return new LoginResponseDto
                {
                    Token = "",
                    Name = user.Name,
                    Role = role,
                    IsFirstLogin = true,
                    IsEmailConfirmed = true,
                    Message = "Password change required"
                };
            }

            var loginResult = await _authService.LoginAsync(dto.Email, dto.Password);

            return loginResult;
        }
    }
}
