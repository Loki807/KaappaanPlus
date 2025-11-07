using KaappaanPlus.Domain.Entities;
using KaappanPlus.Persistence.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace KaappanPlus.Persistence.Seeds
{
    public static class SuperAdminSeeder
    {
        public static async Task SeedSuperAdminAsync(AppDbContext context)
        {
            if (await context.AppUsers.AnyAsync(u => u.Email == "Kaappaan@gmail.com"))
                return;

            var systemTenantId = await SystemTenantSeeder.SeedSystemTenantAsync(context);

            // Create SuperAdmin user
            var superAdmin = new AppUser(
                tenantId: systemTenantId,
                name: "Super Admin",
                email: "Kaappaan@gmail.com",
                phone: "0000000000",
                passwordHash: "",
                role: "SuperAdmin"
            );

            var hasher = new PasswordHasher<AppUser>();
            var passwordHash = hasher.HashPassword(superAdmin, "2025Lk@");
            superAdmin.SetPasswordHash(passwordHash);

            var superAdminRole = await context.Roles.FirstAsync(r => r.Name == "SuperAdmin");
            superAdmin.UserRoles.Add(new UserRole(superAdmin.Id, superAdminRole.Id));

            await context.AddAsync(superAdmin);
            await context.SaveChangesAsync();
        }
    }
}
