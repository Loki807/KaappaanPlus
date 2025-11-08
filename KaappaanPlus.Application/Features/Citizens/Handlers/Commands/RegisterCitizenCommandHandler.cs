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

        public RegisterCitizenCommandHandler(IUserRepository userRepo, ICitizenRepository citizenRepo)
        {
            _userRepo = userRepo;
            _citizenRepo = citizenRepo;
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
                TenantId = null   // 👈 Optional if SuperAdmin-based citizen
            };

            await _userRepo.CreateUserAsync(appUser, cancellationToken);

            // 🧩 Create Citizen profile linked to AppUser
            var citizen = new Citizen(
                appUser.Id,
                dto.NIC,
                dto.Address,
                dto.EmergencyContact
            );

            await _citizenRepo.AddAsync(citizen);

            return appUser.Id;
        }
    }
}
