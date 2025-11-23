using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaappaanPlus.Application.Features.Citizens.DTOs
{
    public class CitizenLoginResponseDto
    {
        public string Token { get; set; } = default!;
        public string FullName { get; set; } = default!;
        public string Message { get; set; } = default!;
        public Guid CitizenId { get; set; }     // ⭐ Required for OTP Verify
        public bool IsEmailConfirmed { get; set; } // ⭐ true → dashboard, false → OTP
    }
}
