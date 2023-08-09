﻿using ECX.Website.Application.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECX.Website.Application.CQRS.Commodities.Request.Command
{
    public class DeleteCommodityCommand : IRequest<BaseCommonResponse>
    {
        public string Id { get; set; }
    }
}
