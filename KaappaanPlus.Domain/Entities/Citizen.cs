using KaappaanPlus.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaappaanPlus.Domain.Entities
{
    public class Citizen : AuditableEntity
    {
        public Guid AppUserId { get; private set; }      // FK to AppUser
        public AppUser AppUser { get; private set; } = default!;

        public string? NIC { get; private set; }
        public string? Address { get; private set; }
        public string? EmergencyContact { get; private set; }
        public Guid TenantId { get; private set; }  // Add this
        private Citizen() { } // EF Core needs this

        // ✅ Constructor
        public Citizen(Guid appUserId, string? nic, string? address, string? emergencyContact = null)
        {
            AppUserId = appUserId;
            NIC = nic;
            Address = address;
            EmergencyContact = emergencyContact;
            SetCreated("system");
        }

        // ✅ Update profile safely
        public void UpdateProfile(string? nic, string? address, string? emergencyContact)
        {
            NIC = nic;
            Address = address;
            EmergencyContact = emergencyContact;
            SetUpdated("system");
        }

        public Citizen(Guid appUserId, string? nic, string? address, Guid tenantId, string? emergencyContact = null)
        {
            AppUserId = appUserId;
            NIC = nic;
            Address = address;
            TenantId = tenantId;
            EmergencyContact = emergencyContact;
            SetCreated("system");
        }

      
    }
}
