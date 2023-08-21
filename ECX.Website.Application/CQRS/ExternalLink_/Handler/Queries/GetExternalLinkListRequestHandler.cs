using AutoMapper;
using ECX.Website.Application.Contracts.Persistence;
using ECX.Website.Application.CQRS.ExternalLink_.Request.Command;
using ECX.Website.Application.DTOs.ExternalLink;
using ECX.Website.Application.DTOs.ExternalLink.Validators;
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
using ECX.Website.Application.CQRS.ExternalLink_.Request.Queries;

namespace ECX.Website.Application.CQRS.ExternalLink_.Handler.Queries
{
    public class GetExternalLinkListRequestHandler : IRequestHandler<GetExternalLinkListRequest, BaseCommonResponse>
    {
        private IExternalLinkRepository _externalLinkRepository;
        private IMapper _mapper;
        public GetExternalLinkListRequestHandler(IExternalLinkRepository externalLinkRepository, IMapper mapper)
        {
            _externalLinkRepository = externalLinkRepository;
            _mapper = mapper;
        }
        public async Task<BaseCommonResponse> Handle(GetExternalLinkListRequest request, CancellationToken cancellationToken)
        {
            var response = new BaseCommonResponse();
            var data = await _externalLinkRepository.GetAll();

            response.Success = true;
            response.Data = _mapper.Map<List<ExternalLinkDto>>(data);
            response.Status = "200";

            return response;
        }
    }
}
