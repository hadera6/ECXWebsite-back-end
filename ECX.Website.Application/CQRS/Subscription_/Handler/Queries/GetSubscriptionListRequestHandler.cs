using AutoMapper;
using ECX.Website.Application.Contracts.Persistence;
using ECX.Website.Application.CQRS.Subscription_.Request.Command;
using ECX.Website.Application.DTOs.Subscription;
using ECX.Website.Application.DTOs.Subscription.Validators;
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
using ECX.Website.Application.CQRS.Subscription_.Request.Queries;

namespace ECX.Website.Application.CQRS.Subscription_.Handler.Queries
{
    public class GetSubscriptionListRequestHandler : IRequestHandler<GetSubscriptionListRequest, BaseCommonResponse>
    {
        private ISubscriptionRepository _subscriptionRepository;
        private IMapper _mapper;
        public GetSubscriptionListRequestHandler(ISubscriptionRepository subscriptionRepository, IMapper mapper)
        {
            _subscriptionRepository = subscriptionRepository;
            _mapper = mapper;
        }
        public async Task<BaseCommonResponse> Handle(GetSubscriptionListRequest request, CancellationToken cancellationToken)
        {
            var response = new BaseCommonResponse();
            var data = await _subscriptionRepository.GetAll();

            response.Success = true;
            response.Data = _mapper.Map<List<SubscriptionDto>>(data);
            response.Status = "200";

            return response;
        }
    }
}
