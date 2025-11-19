using KaappaanPlus.Application.Contracts.Persistence;
using KaappaanPlus.Application.Features.Citizens.DTOs;
using KaappaanPlus.Application.Features.Responders.DTOs;
using KaappaanPlus.WebApi.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

[ApiController]
[Route("api/location")]
public class LocationController : ControllerBase
{
    private readonly ILocationRepository _repo;
    private readonly IHubContext<AlertHub> _hub;

    public LocationController(ILocationRepository repo, IHubContext<AlertHub> hub)
    {
        _repo = repo;
        _hub = hub;
    }

    // ⭐ CITIZEN LOCATION
    [HttpPost("citizen/update")]
    public async Task<IActionResult> UpdateCitizenLocation(CitizenLocationUpdateDto dto)
    {
        await _repo.SaveCitizenLocationAsync(dto.CitizenId, dto.Latitude, dto.Longitude);

        await _hub.Clients.Group(dto.AlertId.ToString())
            .SendAsync("CitizenLocationUpdated", new
            {
                citizenId = dto.CitizenId,
                lat = dto.Latitude,
                lng = dto.Longitude
            });

        return Ok(new { message = "Location updated" });
    }

    // ⭐ RESPONDER LOCATION
    [HttpPost("responder/update")]
    public async Task<IActionResult> UpdateResponderLocation(UpdateResponderLocationDto dto)
    {
        await _repo.SaveResponderLocationAsync(dto.ResponderId, dto.Latitude, dto.Longitude);

        await _hub.Clients.Group(dto.AlertId.ToString())
            .SendAsync("ResponderLocationUpdated", new
            {
                responderId = dto.ResponderId,
                lat = dto.Latitude,
                lng = dto.Longitude
            });

        return Ok(new { message = "Location updated" });
    }
}


