using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaappaanPlus.Application.Features.Citizens.DTOs
{
    public class UpdateCitizenDto
    {
        public Guid Id { get; set; }              // Citizen ID
        public string FullName { get; set; } = default!;
        public string? NIC { get; set; }
        public string? Address { get; set; }
        public string? EmergencyContact { get; set; }
    }
}
