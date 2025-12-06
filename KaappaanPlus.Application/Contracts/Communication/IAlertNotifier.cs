using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaappaanPlus.Application.Contracts.Communication
{
    public interface IAlertNotifier
    {
        Task SendAlertAsync(object payload, string[] roles);

        // 🔥 Notify citizen about responder actions
        Task NotifyCitizenAsync(Guid alertId, object payload);

        // 🔥 Live responder location tracking
        Task SendResponderLocationAsync(Guid alertId, object payload);
    }
}
