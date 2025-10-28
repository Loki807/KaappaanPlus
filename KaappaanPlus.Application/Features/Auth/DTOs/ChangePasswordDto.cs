using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaappaanPlus.Application.Features.Auth.DTOs
{
    public class ChangePasswordDto
    {
        public string Email { get; set; } = default!;
        public string OldPassword { get; set; } = default!;
        public string NewPassword { get; set; } = default!;
    }
}
