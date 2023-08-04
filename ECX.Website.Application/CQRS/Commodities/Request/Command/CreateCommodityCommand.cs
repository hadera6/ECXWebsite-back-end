using ECX.Website.Application.DTOs.Commodity;
using ECX.Website.Application.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECX.Website.Application.CQRS.Commodities.Request.Command
{
    public class CreateCommodityCommand : IRequest<BaseCommonResponse>
    {
        public CommodityFormDto CommodityFormDto { get; set; }
    }
}
