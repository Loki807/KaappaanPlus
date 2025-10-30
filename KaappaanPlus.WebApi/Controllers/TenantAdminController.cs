using KaappaanPlus.Application.Features.Users;
using KaappaanPlus.Application.Features.Users.DTOs;
using KaappaanPlus.Application.Features.Users.Requests.Commands;
using KaappaanPlus.Application.Features.Users.Requests.Query;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KaappaanPlus.WebApi.Controllers
{
    [ApiController]
    [Route("api/tenantadmin")]
   [Authorize(Roles = "TenantAdmin,SuperAdmin")]
    public class TenantAdminController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TenantAdminController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // 🟢 1️⃣ CREATE USER
        // POST: /api/tenantadmin/user/create
        [HttpPost("user/create")]
        public async Task<IActionResult> CreateTenantUser([FromBody] CreateUserDto dto)
        {
            if (dto == null)
                return BadRequest("Invalid user data.");

            // 🔑 Extract TenantId from token (TenantAdmin)
            var tenantIdClaim = User.FindFirst("tenantId")?.Value;
            if (string.IsNullOrEmpty(tenantIdClaim))
                return Unauthorized("Tenant ID missing in token. Please log in again.");

            dto.TenantId = Guid.Parse(tenantIdClaim);

            var command = new CreateUserCommand { UserDto = dto };
            var userId = await _mediator.Send(command);

            return Ok(new
            {
                Success = true,
                Message = "Tenant user created successfully.",
                UserId = userId
            });
        }

        // 🟡 2️⃣ UPDATE USER
        // PUT: /api/tenantadmin/user/update
        [HttpPut("user/update")]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserDto dto)
        {
            if (dto == null)
                return BadRequest("Invalid user data.");

            await _mediator.Send(new UpdateUserCommand { UpdateUserDto = dto });
            return Ok(new
            {
                Success = true,
                Message = "User updated successfully."
            });
        }

        // 🔵 3️⃣ GET USER BY ID
        // GET: /api/tenantadmin/user/{id}
        [HttpGet("user/{id}")]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            var result = await _mediator.Send(new GetUserByIdQuery { Id = id });
            return Ok(new
            {
                Success = true,
                Message = "User retrieved successfully.",
                Data = result
            });
        }

        // 🟣 4️⃣ GET ALL USERS (for this tenant)
        // GET: /api/tenantadmin/users
        [HttpGet("users")]
        public async Task<IActionResult> GetAllUsers()
        {
            var tenantIdClaim = User.FindFirst("tenantId")?.Value;
            if (string.IsNullOrEmpty(tenantIdClaim))
                return Unauthorized("Tenant ID missing in token.");

            var query = new GetUsersByTenantQuery { TenantId = Guid.Parse(tenantIdClaim) };
            var users = await _mediator.Send(query);

            return Ok(new
            {
                Success = true,
                Count = users.Count,
                Data = users
            });
        }

        // 🔴 5️⃣ DELETE USER
        // DELETE: /api/tenantadmin/user/{id}
        [HttpDelete("user/{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            await _mediator.Send(new DeleteUserCommand { Id = id });
            return Ok(new
            {
                Success = true,
                Message = "User deleted successfully."
            });
        }
    }
}
