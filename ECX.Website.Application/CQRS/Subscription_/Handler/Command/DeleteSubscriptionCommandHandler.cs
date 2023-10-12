using AutoMapper;
using ECX.Website.Application.CQRS.Subscription_.Request.Command;
using ECX.Website.Application.Exceptions;
using ECX.Website.Application.Contracts.Persistence;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ECX.Website.Domain;
using ECX.Website.Application.Response;

namespace ECX.Website.Application.CQRS.Subscription_.Handler.Command
{
    public class DeleteSubscriptionCommandHandler : IRequestHandler<DeleteSubscriptionCommand, BaseCommonResponse>
    {
        
        private ISubscriptionRepository _subscriptionRepository;
        private IMapper _mapper;
        public DeleteSubscriptionCommandHandler(ISubscriptionRepository subscriptionRepository, IMapper mapper)
        {
            _subscriptionRepository = subscriptionRepository;
            _mapper = mapper;
        }
        public async Task<BaseCommonResponse> Handle(DeleteSubscriptionCommand request, CancellationToken cancellationToken)
        {
            var data = await _subscriptionRepository.GetById(request.Id);
            var response = new BaseCommonResponse();

            if (data == null)
            {
                response.Success = false;
                response.Message = new NotFoundException(
                            nameof(Subscription), request.Id).Message.ToString();
                response.Status = "404";
            }
            else
            {
                await _subscriptionRepository.Delete(data);

                response.Success = true;
                response.Message = "Successfully Deleted";
                response.Status = "200";

            }
                
            return response;
        }
    }
}
