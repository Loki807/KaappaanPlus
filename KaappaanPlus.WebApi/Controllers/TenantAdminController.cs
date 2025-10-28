using KaappaanPlus.Application.Features.Users;
using KaappaanPlus.Application.Features.Users.DTOs;
using KaappaanPlus.Application.Features.Users.Requests.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KaappaanPlus.WebApi.Controllers
{
    [ApiController]
    [Route("api/tenantadmin")]  // Base route
    [Authorize(Roles = "TenantAdmin,SuperAdmin")] // TenantAdmin + SuperAdmin only
    public class TenantAdminController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TenantAdminController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // ✅ POST: /api/tenantadmin/user/create
        [HttpPost("user/create")]
        public async Task<IActionResult> CreateTenantUser([FromBody] CreateUserDto dto)
        {
            if (dto == null)
                return BadRequest("Invalid user data.");

            // ✅ Extract TenantId from JWT token
            var tenantIdClaim = User.FindFirst("tenantId")?.Value;

            if (string.IsNullOrEmpty(tenantIdClaim))
                return Unauthorized("Tenant ID missing in token. Please log in again.");

            dto.TenantId = Guid.Parse(tenantIdClaim);

            // ✅ Send command through Mediator (Clean architecture)
            var command = new CreateUserCommand { UserDto = dto };
            var userId = await _mediator.Send(command);

            return Ok(new
            {
                Message = "Tenant user created successfully.",
                UserId = userId
            });
        }


        [HttpPut("user/update")]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserDto dto)
        {
            await _mediator.Send(new UpdateUserCommand { UpdateUserDto = dto });
            return Ok(new { Message = "User updated successfully" });
        }


        //// ✅ GET: /api/tenantadmin/users
        //[HttpGet("users")]
        //public async Task<IActionResult> GetAllUsers()
        //{
        //    var tenantIdClaim = User.FindFirst("tenantId")?.Value;

        //    if (string.IsNullOrEmpty(tenantIdClaim))
        //        return Unauthorized("Tenant ID missing in token.");

        //    // Later we’ll create GetUsersByTenantQuery
        //    // For now just show placeholder:
        //    return Ok(new
        //    {
        //        Message = "Users list endpoint working ✅",
        //        TenantId = tenantIdClaim
        //    });
        //}
    }
}
