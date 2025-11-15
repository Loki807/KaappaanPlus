using KaappaanPlus.Application.Contracts.Identity;
using KaappaanPlus.Application.Contracts.Persistence;
using KaappaanPlus.Application.Features.Citizens.Requests.Commands;
using KaappaanPlus.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

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

            // 1️⃣ Check duplicate email
            var existing = await _userRepo.GetByEmailAsync(dto.Email, cancellationToken);
            if (existing != null)
                throw new Exception("Citizen already registered with this email.");

            // 2️⃣ Hash password
            var tempUser = new AppUser(Guid.Empty, dto.FullName, dto.Email, dto.PhoneNumber);
            var hasher = new PasswordHasher<AppUser>();
            var hashedPassword = hasher.HashPassword(tempUser, dto.Password);

            // 3️⃣ Create AppUser
            var appUser = new AppUser(Guid.NewGuid(), dto.FullName, dto.Email, dto.PhoneNumber)
            {
                PasswordHash = hashedPassword,
                Role = "Citizen",
                TenantId = null,
                IsEmailConfirmed = false
            };

            await _userRepo.CreateUserAsync(appUser, cancellationToken);

            // 4️⃣ Create Citizen profile
            var citizen = new Citizen(
                appUser.Id,
                dto.NIC,
                dto.Address,
                dto.EmergencyContact
            );

            await _citizenRepo.AddAsync(citizen);

            // 5️⃣ Generate OTP
            var otp = new Random().Next(100000, 999999).ToString();

            appUser.EmailOtp = otp;
            appUser.OtpExpiryTime = DateTime.UtcNow.AddMinutes(5);

            await _userRepo.UpdateAsync(appUser, cancellationToken);

            // 6️⃣ Send OTP Email
            await _notificationService.SendEmailAsync(
                appUser.Email,
                "Kaappaan - Email Verification",
                $"Your OTP verification code is <b>{otp}</b>. It expires in 5 minutes."
            );

            return appUser.Id;
        }
    }
}
