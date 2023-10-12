using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECX.Website.Application.DTOs.FeedBack.Validators
{
    public class FeedBackDtoValidator : AbstractValidator<FeedBackDto>
    {
        public FeedBackDtoValidator()
        {
            RuleFor(p=>p.Subject)
                .NotEmpty().WithMessage("{PropertyName} is requiered.")
                .NotNull();
            RuleFor(p => p.Comment)
               .NotEmpty().WithMessage("{PropertyName} is requiered.")
               .NotNull();
        }
    }
}
