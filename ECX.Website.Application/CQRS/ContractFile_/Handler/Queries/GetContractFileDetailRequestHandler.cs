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
    public class GetContractFileDetailRequestHandler : IRequestHandler<GetContractFileDetailRequest, BaseCommonResponse>
    {
        private IContractFileRepository _contractFileRepository;
        private IMapper _mapper;
        
        public GetContractFileDetailRequestHandler(IContractFileRepository contractFileRepository, IMapper mapper)
        {
            _contractFileRepository = contractFileRepository;
            _mapper = mapper;
        }
        public async Task<BaseCommonResponse> Handle(GetContractFileDetailRequest request, CancellationToken cancellationToken)
        {
            var response = new BaseCommonResponse();
            var data = await _contractFileRepository.GetById(request.Id);
            if (data != null)
            {
                response.Success = true;
                response.Data = _mapper.Map<ContractFileDto>(data);
                response.Status = "200";
            }
            else
            {
                response.Success = false;
                response.Message = new NotFoundException(
                          nameof(ContractFile), request.Id).Message.ToString();
                response.Status = "404";
            }
            return response;
        }
    }
}
