using FluentValidation;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECX.Website.Application.DTOs.Common.Validators
{
    public class PdfValidator : AbstractValidator<IFormFile>
    {
        public PdfValidator()
        {
            RuleFor(x => x.ContentType)
                .NotNull()
                .Must(x => x.Equals("application/pdf"))
                .WithMessage("File must be Pdf");
        }
    }
}
