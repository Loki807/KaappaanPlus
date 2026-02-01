using KaappaanPlus.Application.Contracts.Communication;
using KaappaanPlus.Application.Contracts.Identity;
using KaappaanPlus.Application.Contracts.Persistence;
using KaappaanPlus.Application.Features.Responders.Request.Commands;
using KaappaanPlus.Domain.Entities;
using MediatR;

public class AcceptAlertHandler : IRequestHandler<AcceptAlertCommand, Guid>
{
    private readonly IAlertRepository _alerts;
    private readonly IAlertResponderRepository _alertResponders;
    private readonly IAlertNotifier _notifier;

    public AcceptAlertHandler(
        IAlertRepository alerts,
        IAlertResponderRepository alertResponders,
        IAlertNotifier notifier)
    {
        _alerts = alerts;
        _alertResponders = alertResponders;
        _notifier = notifier;
    }

    public async Task<Guid> Handle(AcceptAlertCommand req, CancellationToken ct)
    {
        var alert = await _alerts.GetByIdAsync(req.AlertId, ct)
            ?? throw new Exception("Alert not found");

        // Idempotency: If already InProgress, check if we are just re-accepting
        // This simple check prevents most double-tap crashing
        if (alert.Status == "InProgress") 
        {
             // We return success immediately as it's already done
             return alert.Id;
        }

        // Update status to InProgress
        alert.UpdateStatus("InProgress");
        await _alerts.UpdateAsync(alert, ct);

        // Find or create responder mapping
        var mapping = await _alertResponders
            .GetByAlertAndResponderAsync(req.AlertId, req.ResponderId, ct);

        if (mapping == null)
        {
            mapping = new AlertResponder(req.AlertId, req.ResponderId, "Accepted by responder");
            await _alertResponders.AddAsync(mapping, ct);
        }
        else
        {
            mapping.SetUpdated("system");
            await _alertResponders.UpdateAsync(mapping);
        }
    
        // 🔥 Notify the citizen
        await _notifier.NotifyCitizenAsync(req.AlertId, new
        {
            type = "ResponderAccepted",
            alertId = req.AlertId,
            responderId = req.ResponderId,
            message = "Responder accepted your alert",
            timestamp = DateTime.UtcNow
        });

        // 🔥 Notify other responders that this is taken
        string[] responderRoles = { "Police", "Fire", "Ambulance", "UniversityStaff" };
        await _notifier.NotifyRespondersAsync(responderRoles, new
        {
            alertId = req.AlertId,
            status = "Accepted",
            message = "Alert has been accepted by someone else."
        });

        return alert.Id;
    }
}
