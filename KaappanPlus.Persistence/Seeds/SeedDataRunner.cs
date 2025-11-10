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
        public static async Task RunAllAsync(AppDbContext context, ILogger logger)
        {
            try
            {
                logger.LogInformation("🔹 Starting Kaappaan seeding process...");

                await RoleSeeder.SeedRolesAsync(context);
                logger.LogInformation("✅ Roles seeded successfully.");

                await SuperAdminSeeder.SeedSuperAdminAsync(context);
                logger.LogInformation("✅ SuperAdmin created.");

                logger.LogInformation("🎉 Seeding completed successfully.");

                await AlertTypeSeeder.SeedAlertTypesAsync(context);
                logger.LogInformation("✅ AlertTypes seeded successfully.");

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "❌ Seeding failed: {Message}", ex.Message);
            }
        }

    }
}

