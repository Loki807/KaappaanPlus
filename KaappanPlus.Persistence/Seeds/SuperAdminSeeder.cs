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
    public static class SuperAdminSeeder
    {
        public static async Task SeedSuperAdminAsync(AppDbContext context)
        {
            // If SuperAdmin already exists, skip
            if (await context.AppUsers.AnyAsync(u => u.Email == "Kaappaan@gmail.com"))
                return;

            // ✅ Ensure SYSTEM Tenant exists first
            var systemTenantId = await SystemTenantSeeder.SeedSystemTenantAsync(context);

            // Create SuperAdmin user
            var hasher = new PasswordHasher<AppUser>();

            var superAdmin = new AppUser(
                tenantId: systemTenantId,  // ✅ Assigned to SYSTEM Tenant
                name: "Super Admin",
                email: "Kaappaan@gmail.com",
                phone: "0000000000"
            );

            superAdmin.SetPasswordHash(hasher.HashPassword(superAdmin, "2025Lk@")); // ✅ secure hash

            // Assign SuperAdmin role
            var superAdminRole = await context.Roles.FirstAsync(r => r.Name == "SuperAdmin");
            superAdmin.UserRoles.Add(new UserRole(superAdmin.Id, superAdminRole.Id));

            await context.AddAsync(superAdmin);
            await context.SaveChangesAsync();
        }
    }
}

