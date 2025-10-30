using KaappaanPlus.Domain.Entities;
using KaappanPlus.Persistence.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaappanPlus.Persistence.Seeds
{
    public static class TenantAdminSeeder
    {
        public static async Task SeedTenantAdminsAsync(AppDbContext context)
        {
            var tenants = await context.Tenants
                .Where(t => t.Code != "SYSTEM_TENANT" && t.IsActive)
                .ToListAsync();

            if (!tenants.Any()) return;

            var tenantAdminRole = await context.Roles
                .FirstOrDefaultAsync(r => r.Name == "TenantAdmin");

            if (tenantAdminRole == null)
                throw new Exception("TenantAdmin role not found. Please seed roles before seeding tenant admins.");

            var hasher = new PasswordHasher<AppUser>();

            foreach (var tenant in tenants)
            {
                var email = $"admin@{tenant.Name.Replace(" ", "").ToLower()}.com";

                if (await context.AppUsers.AnyAsync(u => u.Email == email))
                    continue;

                // ✅ Create instance first
                var tenantAdmin = new AppUser(
                    tenantId: tenant.Id,
                    name: $"{tenant.Name} Admin",
                    email: email,
                    phone: tenant.ContactNumber ?? "0000000000",
                    passwordHash: "",
                    role: "TenantAdmin"
                );

                // ✅ Hash correctly using instance
                var passwordHash = hasher.HashPassword(tenantAdmin, "Admin@123");
                tenantAdmin.SetPasswordHash(passwordHash);

                tenantAdmin.UserRoles.Add(new UserRole(tenantAdmin.Id, tenantAdminRole.Id));
                await context.AddAsync(tenantAdmin);
            }

            await context.SaveChangesAsync();
        }

    }
}
