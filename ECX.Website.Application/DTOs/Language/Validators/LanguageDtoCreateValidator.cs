﻿using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECX.Website.Application.DTOs.Language.Validators
{
    public class LanguageCreateDtoValidator : AbstractValidator<LanguageFormDto>
    {
        public LanguageCreateDtoValidator()
        {
            RuleFor(p=>p.Name)
                .NotEmpty().WithMessage("{PropertyName} is requiered.")
                .NotNull();
            RuleFor(p=>p.ShortName)
                .NotEmpty().WithMessage("{PropertyName} is requiered.")
                .NotNull();
        }
    }
}
