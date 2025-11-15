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

        public AlertController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // CREATE ALERT (Only Citizens can create alerts)
        [HttpPost("create")]
        [Authorize(Roles = "Citizen")] // Only Citizen role allowed to create an alert
        public async Task<IActionResult> CreateAlert([FromBody] CreateAlertDto createAlertDto)
        {
            if (createAlertDto == null)
            {
                return BadRequest("Invalid alert data.");
            }

            // Create the alert via MediatR
            var createdAlertId = await _mediator.Send(new CreateAlertCommand { Alert= createAlertDto });

            return Ok(new { message = "Alert created successfully", alertId = createdAlertId });
        }








        // GET ALERTS by Citizen (citizenId)
        [HttpGet("citizen/{citizenId}")]
        [Authorize(Roles = "Citizen, SuperAdmin, TenantAdmin")] // Citizens can view their own alerts, SuperAdmin and TenantAdmin can view all alerts
        public async Task<IActionResult> GetAlertsByCitizen(Guid citizenId)
        {
            var alerts = await _mediator.Send(new GetAlertsByCitizenQuery { CitizenId = citizenId });
            return Ok(alerts);
        }

        // GET ALERTS by Tenant (tenantId)
        [HttpGet("tenant/{tenantId}")]
        [Authorize(Roles = "SuperAdmin, TenantAdmin")] // Only SuperAdmin and TenantAdmin can view alerts for a tenant
        public async Task<IActionResult> GetAlertsByTenant(Guid tenantId)
        {
            var alerts = await _mediator.Send(new GetAlertsByTenantIdQuery { TenantId = tenantId });
            return Ok(alerts);
        }

        // DELETE ALERT by Id
        [HttpDelete("{id}")]
        [Authorize(Roles = "SuperAdmin, TenantAdmin")] // Only SuperAdmin and TenantAdmin can delete alerts
        public async Task<IActionResult> DeleteAlert(Guid id)
        {
            var deletedAlertId = await _mediator.Send(new DeleteAlertCommand { Id = id });
            return Ok(new { message = "Alert deleted successfully", alertId = deletedAlertId });
        }

        // UPDATE ALERT by Id
        [HttpPut("{id}")]
        [Authorize(Roles = "SuperAdmin, TenantAdmin")] // Only SuperAdmin and TenantAdmin can update alerts
        public async Task<IActionResult> UpdateAlert(Guid id, [FromBody] UpdateAlertDto updateAlertDto)
        {
            if (id != updateAlertDto.Id)
                return BadRequest("ID mismatch.");

            var updatedAlertId = await _mediator.Send(new UpdateAlertCommand { UpdateAlertDto = updateAlertDto });
            return Ok(new { message = "Alert updated successfully", alertId = updatedAlertId });
        }








        [HttpGet("{alertId}/responders")]
        [Authorize(Roles = "TenantAdmin,Police,Fire,Ambulance,SuperAdmin")]
        public async Task<IActionResult> GetAlertResponders(Guid alertId)
        {
            var list = await _mediator.Send(new GetAlertRespondersQuery { AlertId = alertId });
            return Ok(list);
        }

    }
}
