using FluentValidation;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECX.Website.Application.DTOs.Common.Validators
{
    public class VideoValidator : AbstractValidator<IFormFile>
    {
        public VideoValidator()
        {
            RuleFor(x => x.ContentType)
                .NotNull()
                .Must(x =>  x.Equals("video/mp4") || 
                            x.Equals("video/mov") || 
                            x.Equals("video/wmv") ||
                            x.Equals("video/avi") ||
                            x.Equals("video/mkv") ||
                            x.Equals("video/flv") ||
                            x.Equals("video/webm") 
                    )
                .WithMessage("File must be video");
        }
    }
}
