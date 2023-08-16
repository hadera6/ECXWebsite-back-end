using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECX.Website.Application.DTOs.Image.Validators
{
    public class ImageUpdateDtoValidator : AbstractValidator<ImageFormDto>
    {
        public ImageUpdateDtoValidator()
        {

            RuleFor(p => p.LangId)
                .NotEmpty().WithMessage("{PropertyName} is requiered.")
                .NotNull();
            RuleFor(p => p.Title)
                .NotEmpty().WithMessage("{PropertyName} is requiered.")
                .NotNull();
            RuleFor(p => p.Caption)
               .NotEmpty().WithMessage("{PropertyName} is requiered.")
               .NotNull();
        }
    }
}
