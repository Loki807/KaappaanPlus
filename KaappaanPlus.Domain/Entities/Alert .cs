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
        public Guid TenantId { get; private set; }
        public Guid CreatedByUserId { get; private set; }
        public string Type { get; private set; } = default!;  // e.g., "Accident", "Fire", "Women Safety"
        public string? Description { get; private set; }
        public double Latitude { get; private set; }
        public double Longitude { get; private set; }
        public string Status { get; private set; } = "Pending"; // Pending / Active / Resolved

        // Navigation
        public Tenant Tenant { get; private set; } = default!;
        public AppUser CreatedBy { get; private set; } = default!;
        public ICollection<AlertResponder> Responders { get; private set; } = new List<AlertResponder>();

        private Alert() { } // EF Core needs this

        public Alert(Guid tenantId, Guid createdByUserId, string type, string? description, double lat, double lng)
        {
            TenantId = tenantId;
            CreatedByUserId = createdByUserId;
            Type = type;
            Description = description;
            Latitude = lat;
            Longitude = lng;
        }

        public void UpdateStatus(string status)
        {
            Status = status;
        }
    }
}
