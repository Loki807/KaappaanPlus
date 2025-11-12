using KaappanPlus.Persistence.Data;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaappanPlus.Persistence.Seeds
{
    public static class SeedDataRunner
    {
        public static async Task RunAllAsync(AppDbContext context)
        {
            try
            {
                Console.WriteLine("🔹 Starting Kaappaan seeding process...");

                await RoleSeeder.SeedRolesAsync(context);
                Console.WriteLine("✅ Roles seeded successfully.");

                await SuperAdminSeeder.SeedSuperAdminAsync(context);
                Console.WriteLine("✅ SuperAdmin created.");

                await AlertTypeSeeder.SeedAlertTypesAsync(context);
                Console.WriteLine("✅ AlertTypes seeded successfully.");

                //await AlertResponderSeeder.SeedAlertRespondersAsync(context);
                
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Seeding failed: {ex.Message}");
                throw; // Rethrow to stop startup if seeding fails
            }
        }
    }

}


