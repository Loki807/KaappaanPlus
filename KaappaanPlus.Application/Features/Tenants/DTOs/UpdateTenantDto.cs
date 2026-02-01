using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaappaanPlus.Application.Features.Tenants.DTOs
{
    public class UpdateTenantDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Code { get; set; } = default!;
        public string? AddressLine1 { get; set; }
        public string? AddressLine2 { get; set; }
        public string? City { get; set; }
        public string? StateOrDistrict { get; set; }
        public string? PostalCode { get; set; }
        public string? ContactNumber { get; set; }
        public string? ServiceType { get; set; }
        public string? LogoUrl { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
