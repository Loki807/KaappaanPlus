using MediatR;

namespace KaappaanPlus.Application.Features.Citizens.Requests.Quries
{
    public class GetEmergencyContactQuery : IRequest<string>
    {
        public Guid CitizenId { get; set; }
    }
}
