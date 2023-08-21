using AutoMapper;
using ECX.Website.Application.Contracts.Persistence;
using ECX.Website.Application.CQRS.SocialMedia_.Request.Command;
using ECX.Website.Application.DTOs.SocialMedia;
using ECX.Website.Application.DTOs.SocialMedia.Validators;
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
using ECX.Website.Application.CQRS.SocialMedia_.Request.Queries;

namespace ECX.Website.Application.CQRS.SocialMedia_.Handler.Queries
{
    public class GetSocialMediaListRequestHandler : IRequestHandler<GetSocialMediaListRequest, BaseCommonResponse>
    {
        private ISocialMediaRepository _socialMediaRepository;
        private IMapper _mapper;
        public GetSocialMediaListRequestHandler(ISocialMediaRepository socialMediaRepository, IMapper mapper)
        {
            _socialMediaRepository = socialMediaRepository;
            _mapper = mapper;
        }
        public async Task<BaseCommonResponse> Handle(GetSocialMediaListRequest request, CancellationToken cancellationToken)
        {
            var response = new BaseCommonResponse();
            var data = await _socialMediaRepository.GetAll();

            response.Success = true;
            response.Data = _mapper.Map<List<SocialMediaDto>>(data);
            response.Status = "200";

            return response;
        }
    }
}
