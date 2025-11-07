using FluentValidation;
using KaappaanPlus.Application.Features.Citizens.Requests.Quries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaappaanPlus.Application.Features.Citizens.Validators
{
    public class GetCitizenByIdQueryValidator : AbstractValidator<GetCitizenByIdQuery>
    {
        public GetCitizenByIdQueryValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Citizen ID is required.");
        }
    }
}
