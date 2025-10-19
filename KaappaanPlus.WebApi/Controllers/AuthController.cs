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
        public AuthController(IMediator mediator) => _mediator = mediator;

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponseDto>> Login([FromBody] LoginDto dto)
        {
            var result = await _mediator.Send(new LoginCommand { LoginDto = dto });
            return Ok(result);
        }

        [Authorize]  // ✅ must be logged in
        [HttpGet("me")]
        public IActionResult Me()
        {
            var email = User.FindFirstValue(ClaimTypes.Email)
                         ?? User.FindFirstValue(ClaimTypes.NameIdentifier); // backup

            var role = User.FindFirstValue(ClaimTypes.Role);

            return Ok(new
            {
                Email = email,
                Role = role
            });
        }
    }





}
