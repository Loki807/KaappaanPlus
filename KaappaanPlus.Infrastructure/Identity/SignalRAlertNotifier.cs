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
            // 🔎 FIND NEARBY RESPONDERS (within 5km)
            var nearbyConnectionIds = AlertHub.GetConnectionsNear(lat, lng, 5.0).ToList();

            if (nearbyConnectionIds.Any())
            {
                Console.WriteLine($"📍 [SignalR] Sending targeted alert to {nearbyConnectionIds.Count} nearby responders.");
                await _hubContext.Clients.Clients(nearbyConnectionIds).SendAsync("ReceiveAlert", payload);
                // Also send to the group as a fallback for high-priority or if we want others to see it in history
                // But for now, targeted is better as per user request.
            }
            else
            {
                // FALLBACK: Broadcast to roles if no one is "near" (ensures someone gets help)
                Console.WriteLine("📍 [SignalR] No responders nearby. Broadcasting to all relevant roles.");
                foreach (var role in roles)
                    await _hubContext.Clients.Group(role).SendAsync("ReceiveAlert", payload);
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
