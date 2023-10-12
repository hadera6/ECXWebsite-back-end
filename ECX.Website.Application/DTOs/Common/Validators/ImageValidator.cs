using FluentValidation;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECX.Website.Application.DTOs.Common.Validators
{
    public class ImageValidator : AbstractValidator<IFormFile>
    {
        public ImageValidator()
        {
            RuleFor(x => x.ContentType)
                .NotNull()
                .Must(x =>  x.Equals("image/jpeg") || 
                            x.Equals("image/jpg") || 
                            x.Equals("image/png")
                    )
                .WithMessage("File must be Image");
        }
    }
}
