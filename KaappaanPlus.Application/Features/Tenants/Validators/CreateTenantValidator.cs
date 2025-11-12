using FluentValidation;
using KaappaanPlus.Application.Features.Tenants.DTOs;
using KaappaanPlus.Application.Features.Tenants.Requests.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace KaappaanPlus.Application.Features.Tenants.Validators
{
    public class CreateTenantValidator : AbstractValidator<CreateTenantCommand>
    {
        public CreateTenantValidator()
        {
            RuleFor(x => x.TenantDto.Name)
                .NotEmpty().WithMessage("Tenant name is required.");

            RuleFor(x => x.TenantDto.City)
                .NotEmpty().WithMessage("City is required.");

            RuleFor(x => x.TenantDto.ContactNumber)
                .NotEmpty().WithMessage("Phone number is required.")
                .Must(BeAValidPhoneNumber).WithMessage("Invalid phone number format. Example: +94771234567 or 0771234567");


            RuleFor(x => x.TenantDto.ServiceType)
                .NotEmpty().WithMessage("ServiceType is required.")
                .Must(s => new[] { "Police", "Fire", "Ambulance" }.Contains(s))
                .WithMessage("ServiceType must be one of: Police, Fire, or Ambulance.");
        }

        private bool BeAValidPhoneNumber(string? phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
                return false;

            // Allow +94XXXXXXXXX or 0XXXXXXXXX formats
            var regex = new Regex(@"^(\+94|0)\d{9}$");
            return regex.IsMatch(phone);
        }
    }
}
