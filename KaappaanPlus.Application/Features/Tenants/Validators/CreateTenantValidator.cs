using FluentValidation;
using KaappaanPlus.Application.Features.Tenants.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaappaanPlus.Application.Features.Tenants.Validators
{
    public class CreateTenantValidator : AbstractValidator<CreateTenantDto>
    {
        public CreateTenantValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Tenant name is required")
                .MaximumLength(100).WithMessage("Name cannot exceed 100 characters");

            RuleFor(x => x.City)
                .NotEmpty().WithMessage("City is required")
                .MaximumLength(100);

            RuleFor(x => x.ContactNumber)
                .NotEmpty().WithMessage("Contact number is required")
                .Matches(@"^[0-9]{10}$").WithMessage("Contact number must be 10 digits");

            RuleFor(x => x.AddressLine1)
                .NotEmpty().WithMessage("Address is required");

            RuleFor(x => x.PostalCode)
                .MaximumLength(10)
                .When(x => !string.IsNullOrWhiteSpace(x.PostalCode))
                .WithMessage("Postal code cannot exceed 10 characters");
        }
    }
}
