using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaappaanPlus.Application.Features.Auth.DTOs
{
    public class VerifyOtpResponseDto
    {
        public string Token { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string Role { get; set; } = default!;
        public string Message { get; set; } = default!;

        public Guid CitizenId { get; set; }

    }

}
