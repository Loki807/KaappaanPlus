using FluentValidation;
using KaappaanPlus.Application.Features.Alerts.DTOs;
using KaappaanPlus.Application.Features.Alerts.Requests.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaappaanPlus.Application.Features.Alerts.Validators
{
    public class CreateAlertValidator : AbstractValidator<CreateAlertCommand>
    {
        public CreateAlertValidator()
        {
            RuleFor(x => x.AlertDto.CitizenId)
                .NotEmpty().WithMessage("CitizenId is required.");

            RuleFor(x => x.AlertDto.AlertType)
                .NotEmpty().WithMessage("AlertType is required.")
                .Must(t => new[] { "Police", "Fire", "Ambulance" }.Contains(t))
                .WithMessage("AlertType must be Police, Fire, or Ambulance.");

            RuleFor(x => x.AlertDto.Description)
                .NotEmpty().WithMessage("Description is required.")
                .MaximumLength(200).WithMessage("Description cannot exceed 200 characters.");

            RuleFor(x => x.AlertDto.Location)
                .NotEmpty().WithMessage("Location is required.")
                .MaximumLength(150).WithMessage("Location cannot exceed 150 characters.");
        }
    }
}
