using KaappaanPlus.Application.Contracts.Identity;
using KaappaanPlus.Application.Contracts.Persistence;
using KaappaanPlus.Application.Features.Citizens.DTOs;
using KaappaanPlus.Application.Features.Citizens.Requests.Quries;
using KaappaanPlus.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaappaanPlus.Application.Features.Citizens.Handlers.Queries
{
    public class CitizenLoginQueryHandler : IRequestHandler<CitizenLoginQuery, CitizenLoginResponseDto>
    {
        private readonly IUserRepository _userRepo;
        private readonly ICitizenRepository _citizenRepo;
        private readonly IJwtTokenGenerator _jwt;

        public CitizenLoginQueryHandler(IUserRepository userRepo, ICitizenRepository citizenRepo, IJwtTokenGenerator jwt)
        {
            _userRepo = userRepo;
            _citizenRepo = citizenRepo;
            _jwt = jwt;
        }

        public async Task<CitizenLoginResponseDto> Handle(CitizenLoginQuery request, CancellationToken cancellationToken)
        {
            var dto = request.Login;

            // 🧩 1️⃣ Find AppUser
            var user = await _userRepo.GetByEmailAsync(dto.Email, cancellationToken);
            if (user == null || user.Role != "Citizen")
                throw new UnauthorizedAccessException("Invalid email or password.");

            // 🧩 2️⃣ Verify password
            var hasher = new PasswordHasher<AppUser>();
            var result = hasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);
            if (result == PasswordVerificationResult.Failed)
                throw new UnauthorizedAccessException("Invalid email or password.");

            // 🧩 3️⃣ Generate token
            var token = _jwt.GenerateToken(user, user.Role);

            // 🧩 4️⃣ Get profile (optional)
            var citizenProfile = await _citizenRepo.GetByAppUserIdAsync(user.Id);

            return new CitizenLoginResponseDto
            {
                Token = token,
                FullName = user.Name,
                
                Message = "Citizen login successful ✅"
            };

        }
    }
}
