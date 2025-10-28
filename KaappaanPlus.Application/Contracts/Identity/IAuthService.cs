using KaappaanPlus.Application.Features.Auth.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaappaanPlus.Application.Contracts.Identity
{
    public interface IAuthService
    {
        Task<LoginResponseDto> LoginAsync(string email, string password);
        // (optional later: Task RegisterAsync(...);  — for user registration)




        Task ChangePasswordAsync(string email, string oldPassword, string newPassword); // ✅ Add this
    }



}
