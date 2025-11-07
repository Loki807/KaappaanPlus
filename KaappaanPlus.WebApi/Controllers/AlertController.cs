using KaappaanPlus.Application.Features.Alerts.DTOs;
using KaappaanPlus.Application.Features.Alerts.Requests.Commands;
using KaappaanPlus.Application.Features.Alerts.Requests.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KaappaanPlus.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AlertController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AlertController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreateAlertDto dto)
        {
            var id = await _mediator.Send(new CreateAlertCommand { AlertDto = dto });
            return Ok(new { Message = "Alert sent successfully ✅", AlertId = id });
        }

        [HttpGet("tenant/{tenantId}")]
        public async Task<IActionResult> GetByTenant(Guid tenantId)
        {
            var alerts = await _mediator.Send(new GetAlertsByTenantQuery { TenantId = tenantId });
            return Ok(alerts);
        }


        [HttpGet("citizen/{citizenId}")]
        public async Task<IActionResult> GetByCitizen(Guid citizenId)
        {
            var alerts = await _mediator.Send(new GetAlertsByCitizenQuery { CitizenId = citizenId });
            return Ok(alerts);
        }

        // ✅ 4️⃣ UPDATE ALERT STATUS (TenantAdmin updates alert progress)
        [HttpPut("update-status/{id}")]
        public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] string status)
        {
            await _mediator.Send(new UpdateAlertStatusCommand
            {
                AlertId = id,
                Status = status
            });

            return Ok(new
            {
                Message = $"Alert status updated to {status} ✅"
            });
        }
    }
}
