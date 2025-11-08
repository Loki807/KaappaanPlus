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
            RuleFor(x => x.AlertId)
                .NotEmpty().WithMessage("AlertId is required.");

            RuleFor(x => x.Status)
                .NotEmpty().WithMessage("Status is required.")
                .Must(s => new[] { "Pending", "InProgress", "Resolved" }.Contains(s))
                .WithMessage("Status must be one of: Pending, InProgress, or Resolved.");
        }
    }
}
