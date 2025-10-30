using KaappaanPlus.Application.Contracts;
using KaappaanPlus.Application.Contracts.Identity;
using KaappaanPlus.Application.Features.Auth.DTOs;
using KaappaanPlus.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

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

        // ✅ LOGIN FLOW
        public async Task<LoginResponseDto> LoginAsync(string email, string password)
        {
            var user = await _db.AppUsers
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());

            if (user == null)
                throw new UnauthorizedAccessException("Invalid email or password");

            var hasher = new PasswordHasher<AppUser>();
            var result = hasher.VerifyHashedPassword(user, user.PasswordHash!, password);
            if (result == PasswordVerificationResult.Failed)
                throw new UnauthorizedAccessException("Invalid email or password");

            if (user.MustChangePassword)
            {
                return new LoginResponseDto
                {
                    Token = string.Empty,
                    Name = user.Name,
                    Role = user.Role,
                    Message = "Password change required before login"
                };
            }

            var role = user.UserRoles.FirstOrDefault()?.Role?.Name ?? user.Role;
            var token = _jwt.GenerateToken(user, role);

            return new LoginResponseDto
            {
                Token = token,
                Name = user.Name,
                Role = role,
                Message = "Login successful"
            };
        }

        // ✅ CHANGE PASSWORD FLOW
        public async Task ChangePasswordAsync(string email, string oldPassword, string newPassword)
        {
            var user = await _db.AppUsers.FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
            if (user == null)
                throw new UnauthorizedAccessException("User not found");

            var hasher = new PasswordHasher<AppUser>();
            var verify = hasher.VerifyHashedPassword(user, user.PasswordHash!, oldPassword);
            if (verify == PasswordVerificationResult.Failed)
                throw new UnauthorizedAccessException("Old password is incorrect");

            var newHash = hasher.HashPassword(user, newPassword);
            user.UpdatePassword(newHash);

            // 👇 Mark password change complete
            user.ClearPasswordChangeRequirement();

            _db.UpdateEntity(user);
            await _db.SaveChangesAsync();
        }
    }
}
