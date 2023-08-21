﻿using ECX.Website.Application.DTOs.Video;
using ECX.Website.Application.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECX.Website.Application.CQRS.Video_.Request.Command
{
    public class UpdateVideoCommand :IRequest<BaseCommonResponse>
    {
        public VideoFormDto VideoFormDto { get; set; }

    }
}
