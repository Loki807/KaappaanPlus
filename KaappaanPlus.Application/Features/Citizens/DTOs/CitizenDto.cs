using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaappaanPlus.Application.Features.Citizens.DTOs
{
    public class CitizenDto
    {
        public Guid Id { get; set; }
        public Guid AppUserId { get; set; }

        // 👇 from AppUser
        public string FullName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string PhoneNumber { get; set; } = default!;

        // 👇 from Citizen
        public string? NIC { get; set; }
        public string? Address { get; set; }
        public string? EmergencyContact { get; set; }
    }

}
