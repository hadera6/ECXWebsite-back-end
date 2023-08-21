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
    public class GetSocialMediaDetailRequestHandler : IRequestHandler<GetSocialMediaDetailRequest, BaseCommonResponse>
    {
        private ISocialMediaRepository _socialMediaRepository;
        private IMapper _mapper;
        
        public GetSocialMediaDetailRequestHandler(ISocialMediaRepository socialMediaRepository, IMapper mapper)
        {
            _socialMediaRepository = socialMediaRepository;
            _mapper = mapper;
        }
        public async Task<BaseCommonResponse> Handle(GetSocialMediaDetailRequest request, CancellationToken cancellationToken)
        {
            var response = new BaseCommonResponse();
            var data = await _socialMediaRepository.GetById(request.Id);
            if (data != null)
            {
                response.Success = true;
                response.Data = _mapper.Map<SocialMediaDto>(data);
                response.Status = "200";
            }
            else
            {
                response.Success = false;
                response.Message = new NotFoundException(
                          nameof(SocialMedia), request.Id).Message.ToString();
                response.Status = "404";
            }
            return response;
        }
    }
}
