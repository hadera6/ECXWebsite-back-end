using ECX.Website.Application.DTOs.Commodity;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECX.Website.Application.CQRS.Commodities.Request.Queries
{
    public class GetCommodityDetailRequest :IRequest<CommodityDto>
    {
        public int Id { get; set; }
    }
}
