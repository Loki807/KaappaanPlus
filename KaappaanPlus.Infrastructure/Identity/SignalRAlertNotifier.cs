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

        public async Task SendAlertAsync(object payload, string[] roles)
        {
            foreach (var role in roles)
                await _hubContext.Clients.Group(role).SendAsync("ReceiveAlert", payload);
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
    }
}
