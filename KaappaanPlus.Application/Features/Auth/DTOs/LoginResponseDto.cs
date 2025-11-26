using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaappaanPlus.Application.Features.Auth.DTOs
{
    public class LoginResponseDto
    {
        public string Token { get; set; } = "";
        public string Name { get; set; } = "";
        public string Role { get; set; } = "";
        public bool IsEmailConfirmed { get; set; }
        public string? Message { get; set; }
        public bool IsFirstLogin { get; set; } = true;
        
           // ⭐ ADD THIS
    }

}