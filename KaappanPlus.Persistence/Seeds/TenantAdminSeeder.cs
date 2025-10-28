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
            // 1️⃣ Load all active tenants except system tenant
            var tenants = await context.Tenants
                .Where(t => t.Code != "SYSTEM_TENANT" && t.IsActive)
                .ToListAsync();

            if (!tenants.Any()) return;

            // 2️⃣ Ensure TenantAdmin role exists
            var tenantAdminRole = await context.Roles
                .FirstOrDefaultAsync(r => r.Name == "TenantAdmin");

            if (tenantAdminRole == null)
                throw new Exception("TenantAdmin role not found. Please seed roles before seeding tenant admins.");

            // 3️⃣ Password hasher
            var hasher = new PasswordHasher<AppUser>();

            // 4️⃣ Loop tenants and create admin for each
            foreach (var tenant in tenants)
            {
                var email = $"admin@{tenant.Name.Replace(" ", "").ToLower()}.com";

                // Skip if already exists
                if (await context.AppUsers.AnyAsync(u => u.Email == email))
                    continue;

                var passwordHash = hasher.HashPassword(null, "Admin@123");

                var tenantAdmin = new AppUser(
                    tenantId: tenant.Id,
                    name: $"{tenant.Name} Admin",
                    email: email,
                    phone: tenant.ContactNumber ?? "0000000000",
                    passwordHash: passwordHash,
                    role: "TenantAdmin"
                );

                // Assign role relationship
                tenantAdmin.UserRoles.Add(new UserRole(tenantAdmin.Id, tenantAdminRole.Id));

                await context.AddAsync(tenantAdmin);
            }

            // 5️⃣ Save once after loop
            await context.SaveChangesAsync();
        }
    }
}
