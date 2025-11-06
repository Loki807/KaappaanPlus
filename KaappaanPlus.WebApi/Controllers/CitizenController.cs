using KaappaanPlus.Application.Features.Citizens.DTOs;
using KaappaanPlus.Application.Features.Citizens.Requests.Commands;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KaappaanPlus.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CitizenController : ControllerBase
    {

        private readonly IMediator _mediator;
        public CitizenController(IMediator mediator) => _mediator = mediator;

        [HttpPost("register")]
        public async Task<IActionResult> RegisterCitizen([FromBody] CitizenRegisterDto dto)
        {
            var id = await _mediator.Send(new RegisterCitizenCommand { Citizen = dto });
            return Ok(new { message = "Citizen registered successfully ✅", userId = id });
        }

    }
}
