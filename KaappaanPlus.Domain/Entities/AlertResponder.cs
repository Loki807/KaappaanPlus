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
        public Guid Id { get; private set; } = Guid.NewGuid();

        // ✅ Proper FK columns
        public Guid AlertId { get; private set; }
        public Guid ResponderId { get; private set; }

        public string? ResponseStatus { get; private set; }

        // ✅ Navigation properties
        public Alert Alert { get; private set; } = default!;
        public AppUser Responder { get; private set; } = default!;

        private AlertResponder() { }

        public AlertResponder(Guid alertId, Guid responderId, string? status = null)
        {
            AlertId = alertId;
            ResponderId = responderId;
            ResponseStatus = status ?? "Pending";
        }

        public void UpdateStatus(string newStatus)
        {
            ResponseStatus = newStatus;
        }
    }
}
