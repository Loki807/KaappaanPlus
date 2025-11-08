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

            RuleFor(x => x.AlertDto.AlertTypeName)
                .NotEmpty().WithMessage("AlertTypeName is required.")
                .MaximumLength(50).WithMessage("Alert type name cannot exceed 50 characters.");

            RuleFor(x => x.AlertDto.Description)
                .MaximumLength(200).WithMessage("Description cannot exceed 200 characters.");

            RuleFor(x => x.AlertDto.Latitude)
                .NotEmpty().WithMessage("Latitude is required.")
                .InclusiveBetween(-90, 90).WithMessage("Latitude must be between -90 and 90.");

            RuleFor(x => x.AlertDto.Longitude)
                .NotEmpty().WithMessage("Longitude is required.")
                .InclusiveBetween(-180, 180).WithMessage("Longitude must be between");
        }
    }
}
