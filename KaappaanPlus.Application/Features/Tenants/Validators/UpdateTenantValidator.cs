using FluentValidation;
using KaappaanPlus.Application.Features.Tenants.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaappaanPlus.Application.Features.Tenants.Validators
{
    public class UpdateTenantValidator : AbstractValidator<UpdateTenantDto>
    {
        public UpdateTenantValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Tenant ID is required.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Tenant name is required.")
                .MaximumLength(100);

            RuleFor(x => x.City)
                .NotEmpty().WithMessage("City is required.")
                .MaximumLength(100);

            RuleFor(x => x.ContactNumber)
                .Matches(@"^[0-9]{10}$")
                .When(x => !string.IsNullOrEmpty(x.ContactNumber))
                .WithMessage("Contact number must be 10 digits.");
        }
    }
}
