using KaappaanPlus.Application.Contracts;
using KaappaanPlus.Application.Contracts.Identity;
using KaappaanPlus.Application.Features.Auth.DTOs;
using KaappaanPlus.Application.Features.Auth.Requests;
using KaappaanPlus.Application.Features.Citizens.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace KaappaanPlus.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly INotificationService _notificationService;

        public AuthController(IMediator mediator, INotificationService notificationService)
        {
            _mediator = mediator;
            _notificationService = notificationService;
        }

        // ⭐ LOGIN
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var result = await _mediator.Send(new LoginCommand { LoginDto = dto });

            // ⭐ CITIZEN LOGIN
            if (result is CitizenLoginResponseDto citizenResult)
                return Ok(citizenResult);

            // ⭐ ADMIN / RESPONDER LOGIN
            var adminResult = (LoginResponseDto)result;

            // 👉 Your existing login email notification stays EXACTLY same
            if (adminResult.IsEmailConfirmed == true)
            {
                string loginMessage = $@"
            <h2>Kaappaan Login Alert</h2>
            <p>Hi <strong>{adminResult.Name}</strong>,</p>
            <p>Your account was successfully logged in on 
            <strong>{DateTime.Now:dddd, MMMM dd, yyyy hh:mm tt}</strong>.</p>
            <p>If this was not you, please reset your password immediately.</p>
            <p>Stay safe,<br/>Kaappaan Team</p>";

                await _notificationService.SendEmailAsync(
                    dto.Email,
                    "Kaappaan Login Notification",
                    loginMessage
                );
            }

            return Ok(adminResult);
        }



        // ⭐ ME
        [Authorize]
        [HttpGet("me")]
        public IActionResult Me()
        {
            var email = User.FindFirstValue(ClaimTypes.Email)
                         ?? User.FindFirstValue(ClaimTypes.NameIdentifier);

            var role = User.FindFirstValue(ClaimTypes.Role);

            return Ok(new { Email = email, Role = role });
        }

        // ⭐ CHANGE PASSWORD
        
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
        {
            var success = await _mediator.Send(new ChangePasswordCommand { ChangePasswordDto = dto });

            if (success)
            {
                await _notificationService.SendEmailAsync(
                    dto.Email,
                    "Password Changed",
                    "Your Kaappaan account password was changed successfully."
                );

                return Ok(new { Message = "Password changed successfully" });
            }

            return BadRequest(new { Message = "Password change failed" });
        }

        // ⭐ VERIFY OTP - FIXED
        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOtp([FromBody] VerifyOtpDto dto)
        {
            var response = await _mediator.Send(new VerifyOtpCommand { VerifyOtpDto = dto });

            return Ok(response);  // ⭐ ALWAYS return result (token will be here)
        }
    }
}
