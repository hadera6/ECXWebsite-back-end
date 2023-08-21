using ECX.Website.Application.DTOs.Blog;
using ECX.Website.Application.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECX.Website.Application.CQRS.Blog_.Request.Queries
{
    public class GetBlogListRequest :IRequest<BaseCommonResponse>
    {
       
    }
}
