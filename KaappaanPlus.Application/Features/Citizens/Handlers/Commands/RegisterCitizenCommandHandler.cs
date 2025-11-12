using KaappaanPlus.Application.Contracts.Identity;
using KaappaanPlus.Application.Contracts.Persistence;
using KaappaanPlus.Application.Features.Citizens.Requests.Commands;
using KaappaanPlus.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaappaanPlus.Application.Features.Citizens.Handlers.Commands
{
    public class RegisterCitizenCommandHandler : IRequestHandler<RegisterCitizenCommand, Guid>
    {
        private readonly IUserRepository _userRepo;
        private readonly ICitizenRepository _citizenRepo;
        private readonly INotificationService _notificationService;

        public RegisterCitizenCommandHandler(
            IUserRepository userRepo,
            ICitizenRepository citizenRepo,
            INotificationService notificationService)
        {
            _userRepo = userRepo;
            _citizenRepo = citizenRepo;
            _notificationService = notificationService;
        }

        public async Task<Guid> Handle(RegisterCitizenCommand request, CancellationToken cancellationToken)
        {
            var dto = request.Citizen;

            // 🧩 Check duplicate
            var existing = await _userRepo.GetByEmailAsync(dto.Email, cancellationToken);
            if (existing != null)
                throw new Exception("Citizen already registered with this email.");

            // 🧩 Hash password
            var tempUser = new AppUser(Guid.Empty, dto.FullName, dto.Email, dto.PhoneNumber);
            var hasher = new PasswordHasher<AppUser>();
            var hashed = hasher.HashPassword(tempUser, dto.Password);

            // 🧩 Create AppUser
            var appUser = new AppUser(Guid.NewGuid(), dto.FullName, dto.Email, dto.PhoneNumber)
            {
                PasswordHash = hashed,
                Role = "Citizen",
                TenantId = null // 👈 Optional if SuperAdmin-based citizen
            };

            await _userRepo.CreateUserAsync(appUser, cancellationToken);

            // 🧩 Create Citizen profile linked to AppUser
            var citizen = new Citizen(
                appUser.Id,
                dto.NIC,
                dto.Address,
                dto.EmergencyContact
            );

            // 🧩 Generate and send OTP for email verification
            var otp = new Random().Next(100000, 999999).ToString();
            appUser.EmailOtp = otp;
            appUser.OtpExpiryTime = DateTime.UtcNow.AddMinutes(5);
            appUser.IsEmailConfirmed = false;

            await _userRepo.UpdateAsync(appUser, cancellationToken);

            // Send email
            await _notificationService.SendEmailAsync(
                appUser.Email,
                "Kaappaan - Email Verification Code",
                $"Your verification code is <b>{otp}</b>. It will expire in 5 minutes."
            );

            await _citizenRepo.AddAsync(citizen);

           

            return appUser.Id;
        }
    }
}
