using KaappaanPlus.Application.Contracts;
using KaappaanPlus.Application.Contracts.Identity;
using KaappaanPlus.Application.Features.Auth.DTOs;
using KaappaanPlus.Application.Features.Auth.Requests;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
        [HttpPost("login")]
        public async Task<ActionResult<LoginResponseDto>> Login([FromBody] LoginDto dto)
        {
            var result = await _mediator.Send(new LoginCommand { LoginDto = dto });

            // ❗ If email is not verified do NOT send login email
            if (!result.IsEmailConfirmed)
            {
                return BadRequest(new { message = "Email not verified. Please verify OTP." });
            }

            // 📨 Send login notification email
            string loginMessage = $@"
         <h2>Kaappaan Login Alert</h2>
         <p>Hi <strong>{result.Name}</strong>,</p>
         <p>Your account was successfully logged in on 
         <strong>{DateTime.Now:dddd, MMMM dd, yyyy hh:mm tt}</strong>.</p>
         <p>If this was not you, please reset your password immediately.</p>
         <p>Stay safe,<br/>Kaappaan Team</p>";

            await _notificationService.SendEmailAsync(
                dto.Email,
                "Kaappaan Login Notification",
                loginMessage
            );

            return Ok(result);
        }



        // ✅ ME
        [Authorize]
        [HttpGet("me")]
        public IActionResult Me()
        {
            var email = User.FindFirstValue(ClaimTypes.Email)
                         ?? User.FindFirstValue(ClaimTypes.NameIdentifier);

            var role = User.FindFirstValue(ClaimTypes.Role);
            return Ok(new { Email = email, Role = role });
        }

        // ✅ CHANGE PASSWORD
        [Authorize]
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

        // ✅ VERIFY OTP
        // ✅ VERIFY OTP
        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOtp([FromBody] VerifyOtpDto dto)
        {
            var response = await _mediator.Send(new VerifyOtpCommand { VerifyOtpDto = dto });

            // ❗ No need for `if (!result)` because it's not bool anymore
            if (response == null)
                return BadRequest("Invalid OTP");

            return Ok(response);
        }




    }
}