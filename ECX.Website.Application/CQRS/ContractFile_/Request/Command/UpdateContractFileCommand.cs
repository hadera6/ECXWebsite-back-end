using ECX.Website.Application.DTOs.ContractFile;
using ECX.Website.Application.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECX.Website.Application.CQRS.ContractFile_.Request.Command
{
    public class UpdateContractFileCommand :IRequest<BaseCommonResponse>
    {
        public ContractFileFormDto ContractFileFormDto { get; set; }

    }
}
