using KaappaanPlus.Domain.Entities;
using KaappanPlus.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaappanPlus.Persistence.Seeds
{
    public static class RoleSeeder
    {
        public static async Task SeedRolesAsync(AppDbContext context)
        {
            // ✅ Check if roles already exist
            if (await context.Roles.AnyAsync())
                return; // Roles already seeded

            // ✅ Define all roles
            var roles = new List<Role>
            {
                new Role("SuperAdmin"),
                new Role("TenantAdmin"),
                new Role("Citizen"),
                new Role("Police"),
                new Role("Fire"),
                new Role("Traffic"),
                new Role("Ambulance"),
                new Role("UniversityStaff")

            };

            await context.Roles.AddRangeAsync(roles);
            await context.SaveChangesAsync();
        }
    }
}
