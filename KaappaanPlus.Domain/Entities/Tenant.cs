using KaappaanPlus.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaappaanPlus.Domain.Entities
{

    public sealed class Tenant : AuditableEntity
    {
        public string Name { get; private set; } = default!;
        public string Code { get; set; } = default!;

        public string? AddressLine1 { get; private set; }
        public string? AddressLine2 { get; private set; }
        public string? City { get; private set; }
        public string? StateOrDistrict { get; private set; }
        public string? PostalCode { get; private set; }
        public string? Country { get; private set; } = "Sri Lanka";

        public string? ContactNumber { get; private set; }
        public string? LogoUrl { get; private set; }

        // 🆕 Add this line
        public string ServiceType { get; set; } = default!;  // e.g. "Police", "Fire", "Ambulance"

        public bool IsActive { get; set; } = true;

        private Tenant() { } // EF Core needs this

        public Tenant(
            string name,
            string code,
            string? addressLine1,
            string? addressLine2,
            string? city,
            string? stateOrDistrict,
            string? postalCode,
            string? contactNumber,
            string? serviceType,   // 🆕 added parameter
            string? logoUrl = null
        )
        {
            Name = name;
            Code = code;
            AddressLine1 = addressLine1;
            AddressLine2 = addressLine2;
            City = city;
            StateOrDistrict = stateOrDistrict;
            PostalCode = postalCode;
            ContactNumber = contactNumber;
            LogoUrl = logoUrl;
            ServiceType = serviceType ?? "General"; // fallback if missing

            SetCreated("system");
        }

        public void UpdateAddress(
            string? addressLine1,
            string? addressLine2,
            string? city,
            string? stateOrDistrict,
            string? postalCode
        )
        {
            AddressLine1 = addressLine1;
            AddressLine2 = addressLine2;
            City = city;
            StateOrDistrict = stateOrDistrict;
            PostalCode = postalCode;
            SetUpdated("system");
        }

        public void UpdateContact(string? phone, string? logoUrl)
        {
            ContactNumber = phone;
            LogoUrl = logoUrl;
            SetUpdated("system");
        }

        public void UpdateServiceType(string serviceType)
        {
            ServiceType = serviceType;
            SetUpdated("system");
        }

        public void Deactivate() => IsActive = false;
        public void Activate() => IsActive = true;
    }
}