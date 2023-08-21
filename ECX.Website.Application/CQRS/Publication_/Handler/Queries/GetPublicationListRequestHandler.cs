using AutoMapper;
using ECX.Website.Application.Contracts.Persistence;
using ECX.Website.Application.CQRS.Publication_.Request.Command;
using ECX.Website.Application.DTOs.Publication;
using ECX.Website.Application.DTOs.Publication.Validators;
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
using ECX.Website.Application.CQRS.Publication_.Request.Queries;

namespace ECX.Website.Application.CQRS.Publication_.Handler.Queries
{
    public class GetPublicationListRequestHandler : IRequestHandler<GetPublicationListRequest, BaseCommonResponse>
    {
        private IPublicationRepository _publicationRepository;
        private IMapper _mapper;
        public GetPublicationListRequestHandler(IPublicationRepository publicationRepository, IMapper mapper)
        {
            _publicationRepository = publicationRepository;
            _mapper = mapper;
        }
        public async Task<BaseCommonResponse> Handle(GetPublicationListRequest request, CancellationToken cancellationToken)
        {
            var response = new BaseCommonResponse();
            var data = await _publicationRepository.GetAll();

            response.Success = true;
            response.Data = _mapper.Map<List<PublicationDto>>(data);
            response.Status = "200";

            return response;
        }
    }
}
