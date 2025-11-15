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
            // Check if system tenant already exists
            var systemTenant = await context.Tenants
                .FirstOrDefaultAsync(t => t.Code == "SYSTEM_TENANT");

            if (systemTenant != null)
                return systemTenant.Id;

            // Create a simple system tenant (not police/fire etc.)
            var tenant = new Tenant(
                name: "System Tenant",
                code: "SYSTEM_TENANT",
                addressLine1: "Kaappaan HQ",
                addressLine2: null,
                city: "Colombo",
                stateOrDistrict: "Western",
                postalCode: "00001",
                contactNumber: "0110000000",
                serviceType: "System",
                email: "systemtenant@kaappaan.com",  // Added email here
                logoUrl: null
            );

            await context.Tenants.AddAsync(tenant);
            await context.SaveChangesAsync();

            return tenant.Id;
        }

    }
}
