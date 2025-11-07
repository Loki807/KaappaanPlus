using FluentValidation;
using KaappaanPlus.Application.Features.Citizens.Requests.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaappaanPlus.Application.Features.Citizens.Validators
{
    public class DeleteCitizenCommandValidator : AbstractValidator<DeleteCitizenCommand>
    {
        public DeleteCitizenCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Citizen ID is required for deletion.");
        }
    }
}
