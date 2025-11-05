using FluentValidation;
using KaappaanPlus.Application.Features.Alerts.Requests.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaappaanPlus.Application.Features.Alerts.Validators
{
    public class UpdateAlertStatusValidator : AbstractValidator<UpdateAlertStatusCommand>
    {
        public UpdateAlertStatusValidator()
        {
            // 🧩 Ensure ID is valid
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Alert ID is required.");

            // 🧩 Ensure Status is valid
            RuleFor(x => x.Status)
                .NotEmpty().WithMessage("Status cannot be empty.")
                .Must(BeAValidStatus)
                .WithMessage("Invalid status. Allowed values: Pending, Active, Resolved, Cancelled");
        }

        // ✅ helper method for custom validation
        private bool BeAValidStatus(string status)
        {
            var allowedStatuses = new[] { "Pending", "Active", "Resolved", "Cancelled" };
            return allowedStatuses.Contains(status, StringComparer.OrdinalIgnoreCase);
        }
    }
}
