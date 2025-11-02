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
    [Route("api/admin/tenant/user")] // ✅ Nested under tenant route
    public class TenantUserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TenantUserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // ✅ 1️⃣ Create Tenant User
        // --------------------------------------------------
        // 🔗 POST: https://localhost:7055/api/admin/tenant/user/create
        [Authorize(Roles = "SuperAdmin,TenantAdmin")]
        [HttpPost("create")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDto dto)
        {
            if (dto == null)
                return BadRequest("Invalid user data.");

            var command = new CreateUserCommand { UserDto = dto };
            var userId = await _mediator.Send(command);

            return Ok(new
            {
                Message = "Tenant user created successfully",
                UserId = userId
            });
        }

        // ✅ 2️⃣ Get All Users by Tenant
        // --------------------------------------------------
        // 🔗 GET: https://localhost:7055/api/admin/tenant/user/all/{tenantId}
        [Authorize(Roles = "SuperAdmin,TenantAdmin")]
        [HttpGet("all/{tenantId:guid}")]
        public async Task<IActionResult> GetAllUsers(Guid tenantId)
        {
            var result = await _mediator.Send(new GetAllUsersQuery { TenantId = tenantId });
            return Ok(result);
        }

        // ✅ 3️⃣ Get User by Id
        // --------------------------------------------------
        // 🔗 GET: https://localhost:7055/api/admin/tenant/user/{id}
        [Authorize(Roles = "SuperAdmin,TenantAdmin")]
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            var user = await _mediator.Send(new GetUserByIdQuery { Id = id });
            if (user == null)
                return NotFound(new { Message = "Tenant user not found" });

            return Ok(user);
        }

        // ✅ 4️⃣ Update User
        // --------------------------------------------------
        // 🔗 PUT: https://localhost:7055/api/admin/tenant/user/update/{id}
        [Authorize(Roles = "SuperAdmin,TenantAdmin")]
        [HttpPut("update/{id:guid}")]
        public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UpdateUserDto dto)
        {
            dto.Id = id;
            await _mediator.Send(new UpdateUserCommand { UserDto = dto });
            return Ok(new { Message = "Tenant user updated successfully" });
        }

        // ✅ 5️⃣ Delete User
        // --------------------------------------------------
        // 🔗 DELETE: https://localhost:7055/api/admin/tenant/user/delete/{id}
        [Authorize(Roles = "SuperAdmin,TenantAdmin")]
        [HttpDelete("delete/{id:guid}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            await _mediator.Send(new DeleteUserCommand { Id = id });
            return Ok(new { Message = "Tenant user deleted successfully" });
        }
    }
}
