using ECX.Website.Application.DTOs.Commodity;
using ECX.Website.Application.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECX.Website.Application.CQRS.Commodity_.Request.Command
{
    public class UpdateCommodityCommand :IRequest<BaseCommonResponse>
    {
        public CommodityFormDto CommodityFormDto { get; set; }

    }
}
