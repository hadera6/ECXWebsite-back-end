﻿using ECX.Website.Application.DTOs.Research;
using ECX.Website.Application.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECX.Website.Application.CQRS.Research_.Request.Queries
{
    public class GetResearchDetailRequest :IRequest<BaseCommonResponse>
    {
        public string Id { get; set; }
    }
}
