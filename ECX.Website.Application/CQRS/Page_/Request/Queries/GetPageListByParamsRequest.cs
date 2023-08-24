using ECX.Website.Application.DTOs.Page;
using ECX.Website.Application.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECX.Website.Application.CQRS.Page_.Request.Queries
{
    public class GetPageListByParamsRequest :IRequest<BaseCommonResponse>
    {
        public string LangId { get; set; }
        public string CatagoryId { get; set; }

    }
}
