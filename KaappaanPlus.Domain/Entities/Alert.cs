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
        public Guid CitizenId { get; set; }
        public Guid? TenantId { get; set; }
        public string Description { get; set; } = default!;
        public string Location { get;  set; } = default!;
        public string Status { get; set; } = "Pending"; // Pending / InProgress / Resolved
        public Guid AlertTypeId { get;  set; }
        public AlertType AlertTypeRef { get; set; } = default!;
        public DateTime ReportedAt { get; set; } = DateTime.UtcNow;

        // 🆕 Adding ServiceType here
        public ServiceType ServiceType { get;  set; } // Referring to ServiceType

        // 🆕 Separate latitude and longitude as properties
        public double Latitude { get;  set; }
        public double Longitude { get;  set; }

        public Citizen Citizen { get;  set; } = default!;
        public Tenant? Tenant { get; set; }
        // Updated constructor to accept latitude, longitude, and service type
        public Alert(Guid citizenId, Guid alertTypeId, string description, double latitude, double longitude, ServiceType serviceType)
        {
            CitizenId = citizenId;
            AlertTypeId = alertTypeId;
            Description = description;
            Latitude = latitude;
            Longitude = longitude;
            ServiceType = serviceType;

            // ✅ Auto-generate location string from coordinates
            Location = $"{latitude}, {longitude}";

            SetCreated("system");
        }

        public void UpdateStatus(string status)
        {
            Status = status;
            SetUpdated("system");
        }
    }
}
