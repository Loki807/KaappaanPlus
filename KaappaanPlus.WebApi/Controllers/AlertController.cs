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

        // GET api/alert/citizen/{citizenId}
        [HttpGet("citizen/{citizenId}")]
        public async Task<IActionResult> GetAlertsByCitizen(Guid citizenId)
        {
            var alerts = await _mediator.Send(new GetAlertsByCitizenQuery { CitizenId = citizenId });
            return Ok(alerts);
        }

        // GET api/alert/tenant/{tenantId}
        [HttpGet("tenant/{tenantId}")]
        public async Task<IActionResult> GetAlertsByTenant(Guid tenantId)
        {
            var alerts = await _mediator.Send(new GetAlertsByTenantQuery { TenantId = tenantId });
            return Ok(alerts);
        }

        // DELETE api/alert/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAlert(Guid id)
        {
            var deletedAlertId = await _mediator.Send(new DeleteAlertCommand { Id = id });
            return Ok(new { message = "Alert deleted successfully", alertId = deletedAlertId });
        }

        // PUT api/alert/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAlert(Guid id, [FromBody] UpdateAlertDto updateAlertDto)
        {
            if (id != updateAlertDto.Id)
                return BadRequest("ID mismatch.");

            var updatedAlertId = await _mediator.Send(new UpdateAlertCommand { UpdateAlertDto = updateAlertDto });
            return Ok(new { message = "Alert updated successfully", alertId = updatedAlertId });
        }

        // POST api/alert/create
        [HttpPost("create")]
        [Authorize(Roles = "TenantAdmin, SuperAdmin")]
        public async Task<IActionResult> CreateAlert([FromBody] CreateAlertDto createAlertDto)
        {
            if (createAlertDto == null)
            {
                return BadRequest("Invalid alert data.");
            }

            // Send the request to the mediator for creating the alert
            var createdAlertId = await _mediator.Send(new CreateAlertCommand { Alert= createAlertDto });

            return Ok(new { message = "Alert created successfully", alertId = createdAlertId });
        }


    }
}
