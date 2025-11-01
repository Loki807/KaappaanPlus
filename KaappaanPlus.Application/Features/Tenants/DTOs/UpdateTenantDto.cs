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
        public string? AddressLine1 { get; set; }
        public string? City { get; set; }
        public string? ContactNumber { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
