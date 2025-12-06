using KaappaanPlus.Application.Contracts.Communication;
using KaappaanPlus.Application.Contracts.Identity;
using KaappaanPlus.Application.Contracts.Persistence;
using KaappaanPlus.Application.Features.Responders.Request.Commands;
using MediatR;

public class CompleteAlertHandler : IRequestHandler<CompleteAlertCommand, Guid>
{
    private readonly IAlertRepository _alerts;
    private readonly IAlertNotifier _notifier;

    public CompleteAlertHandler(
        IAlertRepository alerts,
        IAlertNotifier notifier)
    {
        _alerts = alerts;
        _notifier = notifier;
    }

    public async Task<Guid> Handle(CompleteAlertCommand req, CancellationToken ct)
    {
        var alert = await _alerts.GetByIdAsync(req.AlertId, ct)
            ?? throw new Exception("Alert not found");

        alert.UpdateStatus("Completed");
        await _alerts.UpdateAsync(alert, ct);

        // 🔥 Notify citizen
        await _notifier.NotifyCitizenAsync(req.AlertId, new
        {
            type = "Completed",
            alertId = req.AlertId,
            message = "Responder marked alert as completed",
            completedAt = DateTime.UtcNow
        });

        return alert.Id;
    }
}
