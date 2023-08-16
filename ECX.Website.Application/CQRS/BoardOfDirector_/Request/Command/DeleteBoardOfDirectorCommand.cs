using ECX.Website.Application.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECX.Website.Application.CQRS.BoardOfDirector_.Request.Command
{
    public class DeleteBoardOfDirectorCommand : IRequest<BaseCommonResponse>
    {
        public string Id { get; set; }
    }
}
