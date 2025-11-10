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
    public static class AlertTypeSeeder
    {
        public static async Task SeedAlertTypesAsync(AppDbContext context)
        {
            if (await context.AlertTypes.AnyAsync()) return;

            var types = new List<AlertType>
            {
                new AlertType("Fire", "Fire accidents or burns"),
                new AlertType("Accident", "Vehicle or road accidents"),
                new AlertType("WomenSafety", "Emergency for women safety"),
                new AlertType("Crime", "Robbery, murder or criminal activity"),
                new AlertType("Medical", "Medical or health emergencies")
            };

            await context.AlertTypes.AddRangeAsync(types);
            await context.SaveChangesAsync();
        }
    }
}
