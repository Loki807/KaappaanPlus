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
    public static class AlertResponderSeeder
    {
        public static async Task SeedAlertRespondersAsync(AppDbContext context)
        {
            if (await context.AlertResponders.AnyAsync()) return;

            // Fetch all alerts
            var alerts = await context.Alerts.ToListAsync();
            var users = await context.AppUsers.Where(u => u.Role == "Police" || u.Role == "Ambulance").ToListAsync();

            var responders = new List<AlertResponder>();

            //foreach (var alert in alerts)
            //{
            //    var assignedUser = users.FirstOrDefault(u => u.Role == "Police");
            //    if (assignedUser != null)
            //    {
            //        responders.Add(new AlertResponder(alert.Id, assignedUser.Id, "Auto-assigned"));
            //    }

            //    // Additional responders can be added similarly based on alert type
            //}

            await context.AlertResponders.AddRangeAsync(responders);
            await context.SaveChangesAsync();
        }
    }
}
