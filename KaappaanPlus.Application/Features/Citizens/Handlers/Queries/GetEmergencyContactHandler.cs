using KaappaanPlus.Application.Contracts.Persistence;
using KaappaanPlus.Application.Features.Citizens.Requests.Quries;
using MediatR;

namespace KaappaanPlus.Application.Features.Citizens.Handlers.Quries
{
    public class GetEmergencyContactHandler
        : IRequestHandler<GetEmergencyContactQuery, string>
    {
        private readonly ICitizenRepository _citizenRepo;

        public GetEmergencyContactHandler(ICitizenRepository citizenRepo)
        {
            _citizenRepo = citizenRepo;
        }

        public async Task<string> Handle(GetEmergencyContactQuery request, CancellationToken ct)
        {
            var citizen = await _citizenRepo.GetByIdAsync(request.CitizenId);

            if (citizen == null)
                throw new Exception("Citizen not found");

            return citizen.EmergencyContact ?? "";
        }
    }
}
