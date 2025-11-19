using KaappaanPlus.Application.Contracts.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaappaanPlus.Application.Features.Responders.Handlers.Commands
{
    public class UpdateResponderLocationHandler : IRequestHandler<UpdateResponderLocationCommand, bool>
    {
        private readonly IAlertResponderRepository _repo;

        public UpdateResponderLocationHandler(IAlertResponderRepository repo)
        {
            _repo = repo;
        }

        public async Task<bool> Handle(UpdateResponderLocationCommand req, CancellationToken ct)
        {
            var mapping = await _repo.GetByAlertAndResponderAsync(req.AlertId, req.ResponderId)
                          ?? throw new Exception("Responder not assigned");

            mapping.Latitude = req.Latitude;
            mapping.Longitude = req.Longitude;
            mapping.SetUpdated("system");

            await _repo.UpdateAsync(mapping);
            return true;

        }
    }

}
