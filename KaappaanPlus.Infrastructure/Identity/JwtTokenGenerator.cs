using KaappaanPlus.Application.Contracts.Identity;
using KaappaanPlus.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace KaappaanPlus.Infrastructure.Identity
{
    public class JwtTokenGenerator : IJwtTokenGenerator
    {
        private readonly IConfiguration _config;
        public JwtTokenGenerator(IConfiguration config) => _config = config;

        public string GenerateToken(AppUser user, string role)
        {
            var s = _config.GetSection("JwtSettings");
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, role)
                // (optional) new Claim("tenantId", user.TenantId?.ToString() ?? "")
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(s["Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: s["Issuer"],
                audience: s["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(double.Parse(s["ExpiryHours"]!)),
                signingCredentials: creds
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
