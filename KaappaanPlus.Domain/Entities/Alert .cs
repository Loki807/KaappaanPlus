using KaappaanPlus.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaappaanPlus.Domain.Entities
{
    public class Alert : AuditableEntity
    {
       
        public Guid CitizenId { get; private set; }
        public Guid TenantId { get; private set; }

        public string AlertType { get; private set; } = default!;  // Police / Fire / Ambulance
        public string Description { get; private set; } = default!;
        public string Location { get; private set; } = default!;
        public string Status { get; private set; } = "Pending";   // Pending / InProgress / Resolved

        // optional
        public DateTime ReportedAt { get; private set; } = DateTime.UtcNow;

        // navigation
        public Citizen Citizen { get; private set; } = default!;
        public Tenant Tenant { get; private set; } = default!;

        private Alert() { }

        public Alert(Guid citizenId, Guid tenantId, string alertType, string description, string location)
        {
            Id = Guid.NewGuid();
            CitizenId = citizenId;
            TenantId = tenantId;
            AlertType = alertType;
            Description = description;
            Location = location;
            Status = "Pending";
            ReportedAt = DateTime.UtcNow;
            SetCreated("system");
        }

        public void UpdateStatus(string status)
        {
            Status = status;
            SetUpdated("system");
        }
    }
}
