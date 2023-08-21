using AutoMapper;
using ECX.Website.Application.Contracts.Persistence;
using ECX.Website.Application.CQRS.FeedBack_.Request.Command;
using ECX.Website.Application.DTOs.FeedBack;
using ECX.Website.Application.DTOs.FeedBack.Validators;
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
using ECX.Website.Application.CQRS.FeedBack_.Request.Queries;

namespace ECX.Website.Application.CQRS.FeedBack_.Handler.Queries
{
    public class GetFeedBackDetailRequestHandler : IRequestHandler<GetFeedBackDetailRequest, BaseCommonResponse>
    {
        private IFeedBackRepository _feedBackRepository;
        private IMapper _mapper;
        
        public GetFeedBackDetailRequestHandler(IFeedBackRepository feedBackRepository, IMapper mapper)
        {
            _feedBackRepository = feedBackRepository;
            _mapper = mapper;
        }
        public async Task<BaseCommonResponse> Handle(GetFeedBackDetailRequest request, CancellationToken cancellationToken)
        {
            var response = new BaseCommonResponse();
            var data = await _feedBackRepository.GetById(request.Id);
            if (data != null)
            {
                response.Success = true;
                response.Data = _mapper.Map<FeedBackDto>(data);
                response.Status = "200";
            }
            else
            {
                response.Success = false;
                response.Message = new NotFoundException(
                          nameof(FeedBack), request.Id).Message.ToString();
                response.Status = "404";
            }
            return response;
        }
    }
}
