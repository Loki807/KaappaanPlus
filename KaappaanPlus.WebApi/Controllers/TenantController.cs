using KaappaanPlus.Application.Features.Tenants.DTOs;
using KaappaanPlus.Application.Features.Tenants.Requests.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KaappaanPlus.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TenantController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TenantController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Authorize(Roles = "SuperAdmin")]   // ✅ ONLY SuperAdmin can call this now
        [HttpPost("create")]
        public async Task<IActionResult> CreateTenant([FromBody] CreateTenantDto tenantDto)
        {
            var command = new CreateTenantCommand { TenantDto = tenantDto };
            var tenantId = await _mediator.Send(command);
            return Ok(new { Message = "Tenant created successfully", TenantId = tenantId });
        }
    }
}
