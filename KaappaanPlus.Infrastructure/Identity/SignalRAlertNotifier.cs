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
    }
}
