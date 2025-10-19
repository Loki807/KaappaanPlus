using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaappaanPlus.Application.Features.Auth.DTOs
{
    public class LoginResponseDto { 
        public string Token { get; set; } = default!;
        public string Name { get; set; } = default!; 
        public string Role { get; set; } = default!;
    }
}
