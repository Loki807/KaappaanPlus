using KaappaanPlus.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaappaanPlus.Domain.Entities
{
    public class AlertResponder : BaseEntity
    {
        public Guid AlertId { get; private set; }
        public Guid ResponderId { get; private set; }
        public string AssignedBy { get; private set; } = "Auto-Assigned"; // who assigned the alert (could be 'system', 'admin')

        public Alert Alert { get; private set; } = default!;
        public AppUser Responder { get; private set; } = default!;

        public AlertResponder(Guid alertId, Guid responderId, string assignedBy)
        {
            AlertId = alertId;
            ResponderId = responderId;
            AssignedBy = assignedBy;
        }
    }
}
