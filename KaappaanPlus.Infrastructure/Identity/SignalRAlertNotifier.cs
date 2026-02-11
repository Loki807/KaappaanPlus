using KaappaanPlus.Application.Contracts.Communication;
using KaappaanPlus.WebApi.Hubs;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaappaanPlus.Infrastructure.Identity
{
    public class SignalRAlertNotifier : IAlertNotifier
    {
        private readonly IHubContext<AlertHub> _hubContext;

        public SignalRAlertNotifier(IHubContext<AlertHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task SendAlertAsync(object payload, string[] roles, double lat, double lng)
        {
            var adminRoles = new[] { "TenantAdmin", "SuperAdmin" };
            var operationalRoles = roles.Except(adminRoles).ToArray();

            // 1. ALWAYS Notify Admins (Global Visibility)
            foreach (var role in roles.Where(r => adminRoles.Contains(r)))
            {
                await _hubContext.Clients.Group(role).SendAsync("ReceiveAlert", payload);
            }

            // 2. Proximity Logic for Responders (Police, Fire, etc.)
            if (operationalRoles.Any())
            {
                var nearbyConnectionIds = AlertHub.GetConnectionsNear(lat, lng, 5.0).ToList();

                if (nearbyConnectionIds.Any())
                {
                    Console.WriteLine($"📍 [SignalR] Sending targeted alert to {nearbyConnectionIds.Count} nearby responders.");
                    await _hubContext.Clients.Clients(nearbyConnectionIds).SendAsync("ReceiveAlert", payload);
                }
                else
                {
                    // FALLBACK: Broadcast to operational roles if no one is "near"
                    Console.WriteLine("📍 [SignalR] No responders nearby. Broadcasting to all operational roles.");
                    foreach (var role in operationalRoles)
                        await _hubContext.Clients.Group(role).SendAsync("ReceiveAlert", payload);
                }
            }
        }

        // ⭐ Notify citizen about acceptance / completion
        public async Task NotifyCitizenAsync(Guid alertId, object payload)
        {
            string groupName = alertId.ToString().ToLower();
            Console.WriteLine($"🔔 [SignalR] Sending 'CitizenAlertUpdate' to Group: {groupName}");
            await _hubContext.Clients.Group(groupName)
                .SendAsync("CitizenAlertUpdate", payload);
        }

        // ⭐ Live responder location streaming
        public async Task SendResponderLocationAsync(Guid alertId, object payload)
        {
            // Console.WriteLine($"📍 [SignalR] Streaming Location to Group '{alertId}'");
            await _hubContext.Clients.Group(alertId.ToString().ToLower())
                .SendAsync("ResponderLocationUpdated", payload);
        }

        public async Task NotifyRespondersAsync(string[] roles, object payload)
        {
            foreach (var role in roles)
            {
                await _hubContext.Clients.Group(role).SendAsync("AlertHandled", payload);
            }
        }
        public async Task NotifyMissionCancelledAsync(Guid alertId)
        {
            string groupName = alertId.ToString().ToLower();
            Console.WriteLine($"🛑 [SignalR] Mission Cancelled: {groupName}");
            await _hubContext.Clients.Group(groupName).SendAsync("MissionCancelled", new { AlertId = alertId });
        }
    }
}
