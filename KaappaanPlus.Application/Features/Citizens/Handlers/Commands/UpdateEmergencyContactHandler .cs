using KaappaanPlus.Application.Contracts.Persistence;
using KaappaanPlus.Application.Features.Citizens.DTOs;
using KaappaanPlus.Application.Features.Citizens.Requests.Commands;
using MediatR;

namespace KaappaanPlus.Application.Features.Citizens.Handlers.Commands
{
    public class UpdateEmergencyContactHandler : IRequestHandler<UpdateEmergencyContactCommand, bool>
    {
        private readonly ICitizenRepository _citizenRepo;

        public UpdateEmergencyContactHandler(ICitizenRepository citizenRepo)
        {
            _citizenRepo = citizenRepo;
        }

        public async Task<bool> Handle(UpdateEmergencyContactCommand request, CancellationToken ct)
        {
            var dto = request.ContactDto;

            var citizen = await _citizenRepo.GetByIdAsync(dto.CitizenId);
            if (citizen == null)
                throw new Exception("Citizen not found");

            // Only update emergency contact field
            citizen.UpdateProfile(
                citizen.NIC,
                citizen.Address,
                dto.EmergencyContact
            );

            await _citizenRepo.UpdateAsync(citizen, ct);

            return true;
        }
    }
}
