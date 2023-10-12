using AutoMapper;
using ECX.Website.Application.Contracts.Persistence;
using ECX.Website.Application.CQRS.Subscription_.Request.Command;
using ECX.Website.Application.DTOs.Subscription;
using ECX.Website.Application.DTOs.Subscription.Validators;
using ECX.Website.Application.DTOs.Common.Validators;
using ECX.Website.Application.Exceptions;
using ECX.Website.Application.Response;
using ECX.Website.Domain;
using MediatR;

namespace ECX.Website.Application.CQRS.Subscription_.Handler.Command
{
    public class UpdateSubscriptionCommandHandler : IRequestHandler<UpdateSubscriptionCommand, BaseCommonResponse>
    {
        private ISubscriptionRepository _subscriptionRepository;
        private IMapper _mapper;
        public UpdateSubscriptionCommandHandler(ISubscriptionRepository subscriptionRepository, IMapper mapper)
        {
            _subscriptionRepository = subscriptionRepository;
            _mapper = mapper;
        }

        public async Task<BaseCommonResponse> Handle(UpdateSubscriptionCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommonResponse();
            var validator = new SubscriptionUpdateDtoValidator();
            var validationResult = await validator.ValidateAsync(request.SubscriptionFormDto);
            var SubscriptionDto = _mapper.Map<SubscriptionDto>(request.SubscriptionFormDto);
            var flag = await _subscriptionRepository.Exists(request.SubscriptionFormDto.Id);

            if (validationResult.IsValid == false)
            {
                response.Success = false;
                response.Message = "Update Failed";
                response.Errors = validationResult.Errors.Select(x => x.ErrorMessage).ToList();
                response.Status = "400";
            }
            else if (flag == false)
            {

                response.Success = false;
                response.Message = new NotFoundException(
                            nameof(Subscription), request.SubscriptionFormDto.Id).Message.ToString();
                response.Status = "404";
            }
            else 
            {
                var updateData = await _subscriptionRepository.GetById(request.SubscriptionFormDto.Id);
                
                _mapper.Map(SubscriptionDto, updateData);

                var data = await _subscriptionRepository.Update(updateData);

                response.Data = _mapper.Map<SubscriptionDto>(data);
                response.Success = true;
                response.Message = "Updated Successfull";
                response.Status = "200";
            }
            return response;
        }
    }
}

