using AutoMapper;
using ECX.Website.Application.Contracts.Persistence;
using ECX.Website.Application.CQRS.FeedBack_.Request.Command;
using ECX.Website.Application.DTOs.FeedBack;
using ECX.Website.Application.DTOs.FeedBack.Validators;
using ECX.Website.Application.DTOs.Common.Validators;
using ECX.Website.Application.Exceptions;
using ECX.Website.Application.Response;
using ECX.Website.Domain;
using MediatR;

namespace ECX.Website.Application.CQRS.FeedBack_.Handler.Command
{
    public class UpdateFeedBackCommandHandler : IRequestHandler<UpdateFeedBackCommand, BaseCommonResponse>
    {
        private IFeedBackRepository _feedBackRepository;
        private IMapper _mapper;
        public UpdateFeedBackCommandHandler(IFeedBackRepository feedBackRepository, IMapper mapper)
        {
            _feedBackRepository = feedBackRepository;
            _mapper = mapper;
        }

        public async Task<BaseCommonResponse> Handle(UpdateFeedBackCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommonResponse();
            var validator = new FeedBackUpdateDtoValidator();
            var validationResult = await validator.ValidateAsync(request.FeedBackFormDto);
            var FeedBackDto = _mapper.Map<FeedBackDto>(request.FeedBackFormDto);
            var flag = await _feedBackRepository.Exists(request.FeedBackFormDto.Id);

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
                            nameof(FeedBack), request.FeedBackFormDto.Id).Message.ToString();
                response.Status = "404";
            }
            else 
            {
                var updateData = await _feedBackRepository.GetById(request.FeedBackFormDto.Id);
                
                _mapper.Map(FeedBackDto, updateData);

                var data = await _feedBackRepository.Update(updateData);

                response.Data = _mapper.Map<FeedBackDto>(data);
                response.Success = true;
                response.Message = "Updated Successfull";
                response.Status = "200";
            }
            return response;
        }
    }
}

