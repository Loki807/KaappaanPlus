using KaappaanPlus.Application.Features.Tenants.DTOs;
using KaappaanPlus.Application.Features.Tenants.Requests.Commands;
using KaappaanPlus.Application.Features.Tenants.Requests.Queries;
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
        [Authorize(Roles = "SuperAdmin")]
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

        // ✅ GET ALL
        [Authorize(Roles = "SuperAdmin")]
        [HttpGet("all")]
        public async Task<IActionResult> GetAllTenants()
        {
            var result = await _mediator.Send(new GetAllTenantsQuery());
            return Ok(result);
        }

        // ✅ GET BY ID
        [Authorize(Roles = "SuperAdmin")]
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetTenantById(Guid id)
        {
            var tenant = await _mediator.Send(new GetTenantByIdQuery { Id = id });
            if (tenant == null) return NotFound(new { message = "Tenant not found" });
            return Ok(tenant);
        }

        // ✅ UPDATE
        [Authorize(Roles = "SuperAdmin")]
        [HttpPut("update/{id:guid}")]
        public async Task<IActionResult> UpdateTenant(Guid id, [FromBody] UpdateTenantDto dto)
        {
            dto.Id = id;
            await _mediator.Send(new UpdateTenantCommand { TenantDto = dto });
            return Ok(new { message = "Tenant updated successfully" });
        }

        // ✅ DELETE
        [Authorize(Roles = "SuperAdmin")]
        [HttpDelete("delete/{id:guid}")]
        public async Task<IActionResult> DeleteTenant(Guid id)
        {
            await _mediator.Send(new DeleteTenantCommand { Id = id });
            return Ok(new { message = "Tenant deleted successfully  or isactive false" });
        }


    }
}

 