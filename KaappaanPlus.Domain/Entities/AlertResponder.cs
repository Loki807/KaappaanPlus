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

        // 🔹 Foreign Keys
        public Guid AlertId { get; private set; }
        public Guid AppUserId { get; private set; }   // Police / Fire / Ambulance user (Tenant-based)

        // 🔹 Navigation Properties
        public Alert Alert { get; private set; } = default!;
        public AppUser AppUser { get; private set; } = default!;

        // 🔹 Extra Info
        public DateTime AssignedAt { get; private set; } = DateTime.UtcNow;
        public DateTime? RespondedAt { get; private set; }
        public string Status { get; private set; } = "Pending";  // Pending / Acknowledged / Resolved
        public string? Notes { get; private set; }
        public string ServiceType { get; private set; } = default!;

        private AlertResponder() { } // EF Core requirement

        // ✅ Constructor
        public AlertResponder(Guid alertId, Guid appUserId)
        {
            AlertId = alertId;
            AppUserId = appUserId;
            AssignedAt = DateTime.UtcNow;
            Status = "Pending";
        }

        // ✅ Methods
        public void Acknowledge(string? notes = null)
        {
            Status = "Acknowledged";
            Notes = notes;
            RespondedAt = DateTime.UtcNow;
        }

        public void Resolve(string? notes = null)
        {
            Status = "Resolved";
            Notes = notes;
            RespondedAt = DateTime.UtcNow;
        }
        public AlertResponder(Guid alertId, Guid appUserId, string serviceType)
        {
            AlertId = alertId;
            AppUserId = appUserId;
            ServiceType = serviceType;
            AssignedAt = DateTime.UtcNow;
            Status = "Pending";
        }

    }
}
