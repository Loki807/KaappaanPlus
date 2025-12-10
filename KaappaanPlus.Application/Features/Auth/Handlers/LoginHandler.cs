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
        private readonly ICitizenRepository _citizenRepo;     // ⭐ ADDED

        public LoginHandler(
            IUserRepository userRepo,
            IAuthService authService,
            INotificationService notification,
            ICitizenRepository citizenRepo)                    // ⭐ ADDED
        {
            _userRepo = userRepo;
            _authService = authService;
            _notification = notification;
            _citizenRepo = citizenRepo;                       // ⭐ ADDED
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
                // ⭐ GET REAL CITIZEN ROW
                var citizen = await _citizenRepo.GetByUserIdAsync(user.Id);
                if (citizen == null)
                    throw new Exception("Citizen record not found");

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
                    CitizenId = citizen.Id,   // ⭐ FIXED (REAL CITIZEN ID)
                    IsEmailConfirmed = false
                };
            }

            // ⭐ RESPONDER LOGIN  (Police, Fire, Ambulance, Traffic)
            if (role == "Police" || role == "Fire" || role == "Ambulance" || role == "Traffic" || role=="UniversityStaff")
            {
                var login = await _authService.LoginAsync(dto.Email, dto.Password);

                return new LoginResponseDto
                {
                    Token = login.Token,
                    Name = user.Name,
                    Role = user.Role,
                    Message = "Responder login successful",
                    IsEmailConfirmed = true,
                    IsFirstLogin = false

                };
            }


            // ⭐ ADMIN (TENANT ADMIN / SUPER ADMIN)
            if (user.MustChangePassword)
            {
                var login = await _authService.LoginAsync(dto.Email, dto.Password);
                return new LoginResponseDto
                {
                    Token = login.Token,
                    Name = user.Name,
                    Role = user.Role,
                    Message = "Password change required",
                    IsFirstLogin = true,
                    IsEmailConfirmed = true,
                    
                };
            }

            // ⭐ NORMAL ADMIN LOGIN (JWT)qwf
            return await _authService.LoginAsync(dto.Email, dto.Password);
        }
    }
}
