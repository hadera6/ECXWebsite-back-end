using AutoMapper;
using ECX.Website.Application.Contracts.Persistence;
using ECX.Website.Application.CQRS.Research_.Request.Command;
using ECX.Website.Application.DTOs.Research;
using ECX.Website.Application.DTOs.Research.Validators;
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
using ECX.Website.Application.CQRS.Research_.Request.Queries;

namespace ECX.Website.Application.CQRS.Research_.Handler.Queries
{
    public class GetResearchDetailRequestHandler : IRequestHandler<GetResearchDetailRequest, BaseCommonResponse>
    {
        private IResearchRepository _researchRepository;
        private IMapper _mapper;
        
        public GetResearchDetailRequestHandler(IResearchRepository researchRepository, IMapper mapper)
        {
            _researchRepository = researchRepository;
            _mapper = mapper;
        }
        public async Task<BaseCommonResponse> Handle(GetResearchDetailRequest request, CancellationToken cancellationToken)
        {
            var response = new BaseCommonResponse();
            var data = await _researchRepository.GetById(request.Id);
            if (data != null)
            {
                response.Success = true;
                response.Data = _mapper.Map<ResearchDto>(data);
                response.Status = "200";
            }
            else
            {
                response.Success = false;
                response.Message = new NotFoundException(
                          nameof(Research), request.Id).Message.ToString();
                response.Status = "404";
            }
            return response;
        }
    }
}
