using AutoMapper;
using ECX.Website.Application.Contracts.Persistence;
using ECX.Website.Application.CQRS.ContractFile_.Request.Command;
using ECX.Website.Application.DTOs.ContractFile;
using ECX.Website.Application.DTOs.ContractFile.Validators;
using ECX.Website.Application.Exceptions;

using ECX.Website.Application.Response;
using ECX.Website.Domain;
using MediatR;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using ECX.Website.Application.CQRS.ContractFile_.Request.Queries;

namespace ECX.Website.Application.CQRS.ContractFile_.Handler.Queries
{
    public class GetContractFileListRequestHandler : IRequestHandler<GetContractFileListRequest, BaseCommonResponse>
    {
        private IContractFileRepository _contractFileRepository;
        private IMapper _mapper;
        public GetContractFileListRequestHandler(IContractFileRepository contractFileRepository, IMapper mapper)
        {
            _contractFileRepository = contractFileRepository;
            _mapper = mapper;
        }
        public async Task<BaseCommonResponse> Handle(GetContractFileListRequest request, CancellationToken cancellationToken)
        {
            var response = new BaseCommonResponse();
            var data = await _contractFileRepository.GetAll();

            response.Success = true;
            response.Data = _mapper.Map<List<ContractFileDto>>(data);
            response.Status = "200";

            return response;
        }
    }
}
