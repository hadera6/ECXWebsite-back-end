using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECX.Website.Application.DTOs.FeedBack.Validators
{
    public class FeedBackCreateDtoValidator : AbstractValidator<FeedBackFormDto>
    {
        public FeedBackCreateDtoValidator()
        {

            RuleFor(p => p.LangId)
                .NotEmpty().WithMessage("{PropertyName} is requiered.")
                .NotNull();
            RuleFor(p => p.Subject)
               .NotEmpty().WithMessage("{PropertyName} is requiered.")
               .NotNull();
            RuleFor(p => p.Comment)
                .NotEmpty().WithMessage("{PropertyName} is requiered.")
                .NotNull();
        }
    }
}
