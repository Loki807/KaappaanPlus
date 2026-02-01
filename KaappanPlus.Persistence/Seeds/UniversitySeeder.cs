using KaappaanPlus.Domain.Entities;
using KaappanPlus.Persistence.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KaappanPlus.Persistence.Seeds
{
    public static class UniversitySeeder
    {
        public static async Task SeedUniversityTenantAsync(AppDbContext context)
        {
            // 1. Check if University Tenant exists
            var uniTenant = await context.Tenants
                .FirstOrDefaultAsync(t => t.Code == "UOM_TENANT");

            if (uniTenant == null)
            {
                uniTenant = new Tenant(
                    name: "University of Moratuwa",
                    code: "UOM_TENANT",
                    addressLine1: "Katubedda",
                    addressLine2: null,
                    city: "Moratuwa",
                    stateOrDistrict: "Colombo",
                    postalCode: "10400",
                    contactNumber: "0112650301",
                    email: "admin@uom.lk",
                    serviceType: "University",
                    logoUrl: null
                );

                await context.Tenants.AddAsync(uniTenant);
                await context.SaveChangesAsync();
                Console.WriteLine("🏫 University Tenant Created.");
            }

            // 2. Load Roles
            var tenantAdminRole = await context.Roles.FirstAsync(r => r.Name == "TenantAdmin");
            var uniStaffRole = await context.Roles.FirstAsync(r => r.Name == "UniversityStaff");
            var hasher = new PasswordHasher<AppUser>();

            // 3. Create University Admin
            if (!await context.AppUsers.AnyAsync(u => u.Email == "uniadmin@uom.lk"))
            {
                var uniAdmin = new AppUser(
                    tenantId: uniTenant.Id,
                    name: "UOM Admin",
                    email: "uniadmin@uom.lk",
                    phone: "0771234567",
                    passwordHash: "",
                    role: "TenantAdmin"
                );
                uniAdmin.SetPasswordHash(hasher.HashPassword(uniAdmin, "UniAdmin123!"));
                uniAdmin.UserRoles.Add(new UserRole(uniAdmin.Id, tenantAdminRole.Id));
                
                await context.AppUsers.AddAsync(uniAdmin);
                Console.WriteLine("👤 University Admin Created.");
            }

            // 4. Create University Staff Responders
            if (!await context.AppUsers.AnyAsync(u => u.Email == "unistaff1@uom.lk"))
            {
                var staff1 = new AppUser(
                    tenantId: uniTenant.Id,
                    name: "Staff Member Alpha",
                    email: "unistaff1@uom.lk",
                    phone: "0777111222",
                    passwordHash: "",
                    role: "UniversityStaff"
                );
                staff1.SetPasswordHash(hasher.HashPassword(staff1, "UniStaff123!"));
                staff1.UserRoles.Add(new UserRole(staff1.Id, uniStaffRole.Id));

                var staff2 = new AppUser(
                    tenantId: uniTenant.Id,
                    name: "Staff Member Beta",
                    email: "unistaff2@uom.lk",
                    phone: "0777333444",
                    passwordHash: "",
                    role: "UniversityStaff"
                );
                staff2.SetPasswordHash(hasher.HashPassword(staff2, "UniStaff123!"));
                staff2.UserRoles.Add(new UserRole(staff2.Id, uniStaffRole.Id));

                await context.AppUsers.AddRangeAsync(staff1, staff2);
                Console.WriteLine("👮 University Staff Responders Created.");
            }

            await context.SaveChangesAsync();
        }
    }
}
