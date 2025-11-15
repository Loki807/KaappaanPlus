using KaappaanPlus.Application.Features.Citizens.DTOs;
using KaappaanPlus.Application.Features.Citizens.Requests.Commands;
using KaappaanPlus.Application.Features.Citizens.Requests.Quries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
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
            var userId = await _mediator.Send(new RegisterCitizenCommand { Citizen = dto });

            return Ok(new
            {
                message = "OTP sent to your email. Please verify to activate your account.",
                userId = userId
            });
        }


        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCitizenDto dto)
        {
            if (id != dto.Id)
                return BadRequest("ID mismatch.");

            await _mediator.Send(new UpdateCitizenCommand { CitizenDto = dto });
            return Ok(new { Message = "Citizen & AppUser updated successfully ✅" });
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var citizen = await _mediator.Send(new GetCitizenByIdQuery { Id = id });
            return Ok(citizen);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var citizens = await _mediator.Send(new GetAllCitizensQuery());
            return Ok(citizens);
        }

        [HttpDelete("{id}")]
       // [Authorize] // optional — require token
        public async Task<IActionResult> Delete(Guid id)
        {
            await _mediator.Send(new DeleteCitizenCommand { Id = id });
            return Ok(new { Message = "Citizen and linked AppUser deleted successfully ✅" });
        }


    }
}
