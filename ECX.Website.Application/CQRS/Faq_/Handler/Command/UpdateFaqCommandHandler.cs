using AutoMapper;
using ECX.Website.Application.Contracts.Persistence;
using ECX.Website.Application.CQRS.Faq_.Request.Command;
using ECX.Website.Application.DTOs.Faq;
using ECX.Website.Application.DTOs.Faq.Validators;
using ECX.Website.Application.DTOs.Common.Validators;
using ECX.Website.Application.Exceptions;
using ECX.Website.Application.Response;
using ECX.Website.Domain;
using MediatR;

namespace ECX.Website.Application.CQRS.Faq_.Handler.Command
{
    public class UpdateFaqCommandHandler : IRequestHandler<UpdateFaqCommand, BaseCommonResponse>
    {
        private IFaqRepository _faqRepository;
        private IMapper _mapper;
        public UpdateFaqCommandHandler(IFaqRepository faqRepository, IMapper mapper)
        {
            _faqRepository = faqRepository;
            _mapper = mapper;
        }

        public async Task<BaseCommonResponse> Handle(UpdateFaqCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommonResponse();
            var validator = new FaqUpdateDtoValidator();
            var validationResult = await validator.ValidateAsync(request.FaqFormDto);
            var FaqDto = _mapper.Map<FaqDto>(request.FaqFormDto);
            var flag = await _faqRepository.Exists(request.FaqFormDto.Id);

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
                            nameof(Faq), request.FaqFormDto.Id).Message.ToString();
                response.Status = "404";
            }
            else 
            {
                var updateData = await _faqRepository.GetById(request.FaqFormDto.Id);
                
                _mapper.Map(FaqDto, updateData);

                var data = await _faqRepository.Update(updateData);

                response.Data = _mapper.Map<FaqDto>(data);
                response.Success = true;
                response.Message = "Updated Successfull";
                response.Status = "200";
            }
            return response;
        }
    }
}