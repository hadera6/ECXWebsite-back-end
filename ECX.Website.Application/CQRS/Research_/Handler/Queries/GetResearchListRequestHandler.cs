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
    public class GetResearchListRequestHandler : IRequestHandler<GetResearchListRequest, BaseCommonResponse>
    {
        private IResearchRepository _researchRepository;
        private IMapper _mapper;
        public GetResearchListRequestHandler(IResearchRepository researchRepository, IMapper mapper)
        {
            _researchRepository = researchRepository;
            _mapper = mapper;
        }
        public async Task<BaseCommonResponse> Handle(GetResearchListRequest request, CancellationToken cancellationToken)
        {
            var response = new BaseCommonResponse();
            var data = await _researchRepository.GetAll();

            response.Success = true;
            response.Data = _mapper.Map<List<ResearchDto>>(data);
            response.Status = "200";

            return response;
        }
    }
}
