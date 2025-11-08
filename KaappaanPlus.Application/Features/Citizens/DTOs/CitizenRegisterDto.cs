using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaappaanPlus.Application.Features.Citizens.DTOs
{
    public class CitizenRegisterDto
    {
        public string FullName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string PhoneNumber { get; set; } = default!;
        public string Password { get; set; } = default!;
        public string? NIC { get; set; }
        public string? Address { get; set; }
        public string? EmergencyContact { get; set; }
    }
}
