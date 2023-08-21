using ECX.Website.Application.DTOs.TradingCenter;
using ECX.Website.Application.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECX.Website.Application.CQRS.TradingCenter_.Request.Queries
{
    public class GetTradingCenterDetailRequest :IRequest<BaseCommonResponse>
    {
        public string Id { get; set; }
    }
}
