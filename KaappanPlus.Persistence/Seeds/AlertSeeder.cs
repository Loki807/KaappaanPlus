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
    public static class AlertSeeder
    {
        public static async Task SeedAlertsAsync(AppDbContext context)
        {
            if (await context.Alerts.AnyAsync()) return;

            var alertTypes = await context.AlertTypes.ToListAsync();

            var alerts = new List<Alert>
            {
                new Alert(
                    Guid.NewGuid(), // CitizenId
                    alertTypes.FirstOrDefault(t => t.Name == "Fire")?.Id ?? Guid.NewGuid(), // AlertTypeId
                    "Fire at Market Road",
                    9.6618,
                    80.0089,
                    ServiceType.Fire
                ),
                new Alert(
                    Guid.NewGuid(), // CitizenId
                    alertTypes.FirstOrDefault(t => t.Name == "Accident")?.Id ?? Guid.NewGuid(), // AlertTypeId
                    "Car accident on Main Street",
                    9.6750,
                    80.0334,
                    ServiceType.Ambulance
                )
            };

            await context.Alerts.AddRangeAsync(alerts);
            await context.SaveChangesAsync();
        }

    }
}
// done 