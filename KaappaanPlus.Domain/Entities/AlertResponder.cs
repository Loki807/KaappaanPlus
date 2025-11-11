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
        public Alert Alert { get; private set; } = default!;

        public Guid ResponderId { get; private set; }
        public AppUser Responder { get; private set; } = default!;

        public string AssignmentReason { get; private set; }

        // Constructor to create a new AlertResponder
        public AlertResponder(Guid alertId, Guid responderId, string assignmentReason)
        {
            AlertId = alertId;
            ResponderId = responderId;
            AssignmentReason = assignmentReason;
        }
    }
}
