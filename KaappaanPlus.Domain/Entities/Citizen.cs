using KaappaanPlus.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaappaanPlus.Domain.Entities
{
    public class Citizen : AuditableEntity
    {
        public Guid AppUserId { get; private set; }
        public AppUser AppUser { get; private set; } = default!;

        public string? NIC { get; private set; }
        public string? Address { get; private set; }
        public string? EmergencyContact { get; private set; }

        private Citizen() { }

        public Citizen(Guid appUserId, string? nic, string? address, string? emergencyContact = null)
        {
            AppUserId = appUserId;
            NIC = nic;
            Address = address;
            EmergencyContact = emergencyContact;

            SetCreated("system");
        }

        public void UpdateProfile(string? nic, string? address, string? emergencyContact)
        {
            NIC = nic;
            Address = address;
            EmergencyContact = emergencyContact;
            SetUpdated("system");
        }
    }

}
