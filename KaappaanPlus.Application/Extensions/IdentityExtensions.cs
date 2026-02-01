using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace KaappaanPlus.Application.Extensions
{
    public static class IdentityExtensions
    {
        public static Guid GetUserId(this ClaimsPrincipal user)
        {
            // 1. Get all potential ID claims (NameIdentifier, sub, uid)
            var candidates = user.FindAll(ClaimTypes.NameIdentifier)
                .Concat(user.FindAll("sub"))
                .Concat(user.FindAll("uid"))
                .Select(c => c.Value);
            
            // 2. Try to find the first one that is a valid GUID
            foreach (var value in candidates)
            {
                if (Guid.TryParse(value, out var guid))
                {
                    return guid;
                }
            }

            // 3. Fallback: Log what we found for debugging (optional in prod)
            var foundValues = string.Join(", ", candidates);
            
            throw new UnauthorizedAccessException($"User ID not found. No valid GUID in claims. Found: [{foundValues}]");
        }
    }

}
