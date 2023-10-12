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
using System.IO;
using ECX.Website.Application.DTOs.Common.Validators;

namespace ECX.Website.Application.CQRS.Subscription_.Handler.Command
{
    public class CreateSubscriptionCommandHandler : IRequestHandler<CreateSubscriptionCommand, BaseCommonResponse>
    {
        private ISubscriptionRepository _subscriptionRepository;
        private IMapper _mapper;

        public CreateSubscriptionCommandHandler(ISubscriptionRepository subscriptionRepository, IMapper mapper)
        {
            _subscriptionRepository = subscriptionRepository;
            _mapper = mapper;
        }
        public async Task<BaseCommonResponse> Handle(CreateSubscriptionCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommonResponse();
            var validator = new SubscriptionCreateDtoValidator();
            var validationResult = await validator.ValidateAsync(request.SubscriptionFormDto);
            var SubscriptionDto = _mapper.Map<SubscriptionDto>(request.SubscriptionFormDto);
            if (validationResult.IsValid == false)
            {
                response.Success = false;
                response.Message = "Creation Faild";
                response.Errors = validationResult.Errors.Select(x => x.ErrorMessage).ToList();
                response.Status = "400";
            }
            else
            {
                string subscriptionId;
                bool flag = true;

                while (true)
                {
                    subscriptionId = Guid.NewGuid().ToString();
                    flag = await _subscriptionRepository.Exists(subscriptionId);
                    if (flag == false)
                    {
                        SubscriptionDto.Id = subscriptionId;
                        break;
                    }
                }

                var data = _mapper.Map<Subscription>(SubscriptionDto);

                var saveData = await _subscriptionRepository.Add(data);

                response.Data = _mapper.Map<SubscriptionDto>(saveData);
                response.Success = true;
                response.Message = "Created Successfully";
                response.Status = "200";
                    
            }
            return response;
        }
    }
}
