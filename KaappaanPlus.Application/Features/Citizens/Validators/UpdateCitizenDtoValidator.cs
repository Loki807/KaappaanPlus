using FluentValidation;
using KaappaanPlus.Application.Features.Citizens.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaappaanPlus.Application.Features.Citizens.Validators
{
    public class UpdateCitizenDtoValidator : AbstractValidator<UpdateCitizenDto>
    {
        public UpdateCitizenDtoValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Citizen ID is required.");

            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("Full name cannot be empty.")
                .MaximumLength(100);

            RuleFor(x => x.NIC)
                .NotEmpty().WithMessage("NIC cannot be empty.");

            RuleFor(x => x.Address)
                .NotEmpty().WithMessage("Address cannot be empty.");

            RuleFor(x => x.EmergencyContact)
                .NotEmpty().WithMessage("Emergency contact is required.")
                .Matches(@"^(07\d{8})$")
                .WithMessage("Emergency contact must start with 07 and have 10 digits.");
        }
    }
}
