using KaappaanPlus.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaappaanPlus.Domain.Entities
{
    public class AlertResponder : AuditableEntity
    {
        public Guid AlertId { get; private set; }
        public Guid ResponderUserId { get; private set; }
        public string? ResponseStatus { get; private set; } = "NotResponded"; // NotResponded / Accepted / Completed

        // Navigation
        public Alert Alert { get; private set; } = default!;
        public AppUser Responder { get; private set; } = default!;

        private AlertResponder() { }

        public AlertResponder(Guid alertId, Guid responderUserId)
        {
            AlertId = alertId;
            ResponderUserId = responderUserId;
        }

        public void UpdateResponse(string status)
        {
            ResponseStatus = status;
        }
    }
}
