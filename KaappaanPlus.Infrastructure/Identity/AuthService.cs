using KaappaanPlus.Application.Contracts;
using KaappaanPlus.Application.Contracts.Identity;
using KaappaanPlus.Application.Features.Auth.DTOs;
using KaappaanPlus.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaappaanPlus.Infrastructure.Identity
{
    public class AuthService : IAuthService
    {
        private readonly IAppDbContext _db;
        private readonly IJwtTokenGenerator _jwt;

        public AuthService(IAppDbContext db, IJwtTokenGenerator jwt)
        {
            _db = db;
            _jwt = jwt;
        }

        public async Task<LoginResponseDto> LoginAsync(string email, string password)
        {
            // 1) FIND USER by email
            var user = await _db.AppUsers
                .Include(u => u.UserRoles).ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.Email == email);

            if (user == null)
                throw new UnauthorizedAccessException("Invalid email or password");

            // 2) VERIFY PASSWORD SECURELY
            var hasher = new PasswordHasher<AppUser>();
            var verification = hasher.VerifyHashedPassword(user, user.PasswordHash!, password);
            if (verification == PasswordVerificationResult.Failed)
                throw new UnauthorizedAccessException("Invalid email or password");

            // 3) GET ROLE
            var role = user.UserRoles.First().Role.Name;

            // 4) GENERATE JWT TOKEN
            var token = _jwt.GenerateToken(user, role);

            // 5) RETURN CLEAN RESPONSE
            return new LoginResponseDto
            {
                Token = token,
                Name = user.Name,
                Role = role
            };
        }
    }
}
