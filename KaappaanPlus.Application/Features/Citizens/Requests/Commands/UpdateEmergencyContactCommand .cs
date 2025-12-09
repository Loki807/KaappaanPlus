using KaappaanPlus.Application.Features.Citizens.DTOs;
using MediatR;

namespace KaappaanPlus.Application.Features.Citizens.Requests.Commands
{
    public class UpdateEmergencyContactCommand : IRequest<bool>
    {
        public UpdateEmergencyContactDto ContactDto { get; set; } = default!;
    }
}
