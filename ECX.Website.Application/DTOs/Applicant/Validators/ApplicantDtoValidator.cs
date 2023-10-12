using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECX.Website.Application.DTOs.Applicant.Validators
{
    public class ApplicantDtoValidator : AbstractValidator<ApplicantDto>
    {
        public ApplicantDtoValidator()
        {
            RuleFor(p=>p.FName)
                .NotEmpty().WithMessage("{PropertyName} is requiered.")
                .NotNull();
            RuleFor(p => p.LName)
               .NotEmpty().WithMessage("{PropertyName} is requiered.")
               .NotNull();
        }
    }
}
