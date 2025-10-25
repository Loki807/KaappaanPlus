using KaappaanPlus.Application.Features.Tenants.DTOs;
using KaappaanPlus.Application.Features.Tenants.Requests.Commands;
using KaappaanPlus.Application.Features.Users;
using KaappaanPlus.Application.Features.Users.Requests.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KaappaanPlus.WebApi.Controllers
{
    [ApiController]
    [Route("api/admin/tenant")] // ✅ shared base path for all tenant-admin actions
    public class TenantController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TenantController(IMediator mediator)
        {
            _mediator = mediator;
        }

        //https://localhost:7055/api/admin/tenant/create =====================================================================
        // ✅ 1️⃣ SuperAdmin → Create new tenant
        // =====================================================================
        //[Authorize(Roles = "SuperAdmin")]
        [HttpPost("create")]
        public async Task<IActionResult> CreateTenant([FromBody] CreateTenantDto tenantDto)
        {
            if (tenantDto == null)
                return BadRequest("Invalid tenant data.");

            var command = new CreateTenantCommand { TenantDto = tenantDto };
            var tenantId = await _mediator.Send(command);

            return Ok(new
            {
                Message = "Tenant created successfully",
                TenantId = tenantId
            });
        }

        //https://localhost:7055/api/admin/tenant/user/create =====================================================================
        // ✅ 2️⃣ TenantAdmin → Create user under their own tenant
        // =====================================================================
        [Authorize(Roles = "TenantAdmin,SuperAdmin")] // optional multi-role access
        [HttpPost("user/create")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDto userDto)
        {
            if (userDto == null)
                return BadRequest("Invalid user data.");

            var command = new CreateUserCommand { UserDto = userDto };
            var userId = await _mediator.Send(command);

            return Ok(new
            {
                Message = "User created successfully",
                UserId = userId
            });
        }
    }
}

