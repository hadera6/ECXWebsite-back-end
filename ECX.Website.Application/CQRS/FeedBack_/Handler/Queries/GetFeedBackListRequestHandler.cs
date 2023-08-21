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
    public class GetFeedBackListRequestHandler : IRequestHandler<GetFeedBackListRequest, BaseCommonResponse>
    {
        private IFeedBackRepository _feedBackRepository;
        private IMapper _mapper;
        public GetFeedBackListRequestHandler(IFeedBackRepository feedBackRepository, IMapper mapper)
        {
            _feedBackRepository = feedBackRepository;
            _mapper = mapper;
        }
        public async Task<BaseCommonResponse> Handle(GetFeedBackListRequest request, CancellationToken cancellationToken)
        {
            var response = new BaseCommonResponse();
            var data = await _feedBackRepository.GetAll();

            response.Success = true;
            response.Data = _mapper.Map<List<FeedBackDto>>(data);
            response.Status = "200";

            return response;
        }
    }
}
