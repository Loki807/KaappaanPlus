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

        [HttpPost]
        public async Task<IActionResult> CreateAlert([FromBody] CreateAlertDto dto)
        {
            // Check if the input dto is valid
            if (dto == null || string.IsNullOrEmpty(dto.AlertTypeName) || string.IsNullOrEmpty(dto.Description))
            {
                return BadRequest("Invalid alert details.");
            }

            try
            {
                // Send the CreateAlertCommand to the handler
                var alertId = await _mediator.Send(new CreateAlertCommand { Alert = dto });
                return Ok(new { message = "Alert created successfully", alertId = alertId });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred", error = ex.Message });
            }
        }


    }
}
