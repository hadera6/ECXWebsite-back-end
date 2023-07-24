using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECX.Website.Application.CQRS.Commodities.Request.Command
{
    public class DeleteCommodityCommand : IRequest
    {
        public int Id { get; set; }
    }
}
