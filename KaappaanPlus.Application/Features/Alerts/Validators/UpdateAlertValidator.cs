using FluentValidation;
using KaappaanPlus.Application.Features.Alerts.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaappaanPlus.Application.Features.Alerts.Validators
{
    public class UpdateAlertValidator : AbstractValidator<UpdateAlertDto>
    {
        public UpdateAlertValidator()
        {
            // Validate the AlertId (it should be provided)
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("AlertId is required.");

            // Validate AlertTypeName (ensure it's a valid value)
            RuleFor(x => x.AlertTypeName)
                .NotEmpty().WithMessage("AlertTypeName is required.")
                .Must(x => new[] { "Accident", "Fire", "Crime", "Medical", "WomenSafety", "Disaster" }.Contains(x))
                .WithMessage("Invalid AlertTypeName. Accepted values: Accident, Fire, Crime, Medical, WomenSafety, Disaster");

            // Validate Description (ensure it's not empty and has a max length)
            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required.")
                .MaximumLength(500).WithMessage("Description cannot exceed 500 characters.");

            // Validate Latitude (ensure it is within the valid range)
            RuleFor(x => x.Latitude)
                .InclusiveBetween(-90, 90).WithMessage("Latitude must be between -90 and 90.");

            // Validate Longitude (ensure it is within the valid range)
            RuleFor(x => x.Longitude)
                .InclusiveBetween(-180, 180).WithMessage("Longitude must be between -180 and 180.");

            // Validate Status (must be one of the valid statuses)
            RuleFor(x => x.Status)
                .NotEmpty().WithMessage("Status is required.")
                .Must(status => new[] { "Pending", "InProgress", "Resolved" }.Contains(status))
                .WithMessage("Status must be either 'Pending', 'InProgress', or 'Resolved'.");
        }
    }
}
