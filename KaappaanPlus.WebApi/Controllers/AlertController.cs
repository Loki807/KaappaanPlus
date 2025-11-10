using KaappaanPlus.Application.Contracts.Persistence;
using KaappaanPlus.Application.Features.Alerts.DTOs;
using KaappaanPlus.Application.Features.Alerts.Requests.Commands;
using KaappaanPlus.Application.Features.Alerts.Requests.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KaappaanPlus.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlertController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IAlertRepository _alertRepo;

        public AlertController(IMediator mediator, IAlertRepository alertRepo)
        {
            _mediator = mediator;
            _alertRepo = alertRepo;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateAlert([FromBody] CreateAlertDto dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.AlertTypeName))
                return BadRequest("Invalid alert details.");

            var id = await _mediator.Send(new CreateAlertCommand { AlertDto = dto });
            return Ok(new { message = "Alert sent successfully.", alertId = id });
        }

        [HttpGet("citizen/{citizenId}")]
        public async Task<IActionResult> GetByCitizen(Guid citizenId)
        {
            var alerts = await _alertRepo.GetByCitizenAsync(citizenId);
            return Ok(alerts);
        }

        [HttpGet("tenant/{tenantId}")]
        public async Task<IActionResult> GetByTenant(Guid tenantId)
        {
            var alerts = await _alertRepo.GetByTenantAsync(tenantId);
            return Ok(alerts);
        }

        [HttpPut("update-status/{id}")]
        public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] string status)
        {
            await _mediator.Send(new UpdateAlertStatusCommand { AlertId = id, Status = status });
            return Ok(new { message = $"Alert status updated to {status}" });
        }
    }

}
