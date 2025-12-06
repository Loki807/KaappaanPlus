using KaappaanPlus.Application.Contracts.Communication;
using KaappaanPlus.Application.Contracts.Identity;
using KaappaanPlus.Application.Contracts.Persistence;
using KaappaanPlus.Application.Features.Responders.Handlers.Commands;
using KaappaanPlus.Application.Features.Responders.Request.Commands;
using MediatR;

public class UpdateResponderLocationHandler
    : IRequestHandler<UpdateResponderLocationCommand, bool>
{
    private readonly IAlertResponderRepository _alertResponders;
    private readonly IAlertNotifier _notifier;

    public UpdateResponderLocationHandler(
        IAlertResponderRepository alertResponders,
        IAlertNotifier notifier)
    {
        _alertResponders = alertResponders;
        _notifier = notifier;
    }

    public async Task<bool> Handle(UpdateResponderLocationCommand req, CancellationToken ct)
    {
        var mapping = await _alertResponders
            .GetByAlertAndResponderAsync(req.AlertId, req.ResponderId, ct)
            ?? throw new Exception("Responder not assigned to this alert");

        mapping.Latitude = req.Latitude;
        mapping.Longitude = req.Longitude;
        mapping.SetUpdated("system");

        await _alertResponders.UpdateAsync(mapping);

        // 🔥 Notify citizen with live GPS point
        await _notifier.SendResponderLocationAsync(req.AlertId, new
        {
            type = "LocationUpdate",
            alertId = req.AlertId,
            responderId = req.ResponderId,
            latitude = req.Latitude,
            longitude = req.Longitude,
            updatedAt = DateTime.UtcNow
        });

        return true;
    }
}
