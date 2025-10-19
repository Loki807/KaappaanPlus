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
    public static class SystemTenantSeeder
    {
        public static async Task<Guid> SeedSystemTenantAsync(AppDbContext context)
        {
            // If System tenant already exists, return its ID
            var existing =
                await context.Tenants.FirstOrDefaultAsync(t => t.Code == "SYS_APP");

            if (existing != null)
                return existing.Id;

            // Create new SYSTEM tenant
            var tenant = new Tenant(
                name: "System Tenant",
                code: "SYS_APP",
                addressLine1: "HQ",
                addressLine2: null,
                city: "SYSTEM",
                stateOrDistrict: "SYSTEM",
                postalCode: "00000",
                contactNumber: "0000000000"
            );

            await context.AddAsync(tenant);
            await context.SaveChangesAsync();

            return tenant.Id; // return its ID for SuperAdmin
        }
    }
}
