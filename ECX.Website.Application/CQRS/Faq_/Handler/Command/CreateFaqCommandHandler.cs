using AutoMapper;
using ECX.Website.Application.Contracts.Persistence;
using ECX.Website.Application.CQRS.Faq_.Request.Command;
using ECX.Website.Application.DTOs.Faq;
using ECX.Website.Application.DTOs.Faq.Validators;
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

namespace ECX.Website.Application.CQRS.Faq_.Handler.Command
{
    public class CreateFaqCommandHandler : IRequestHandler<CreateFaqCommand, BaseCommonResponse>
    {
        private IFaqRepository _faqRepository;
        private IMapper _mapper;

        public CreateFaqCommandHandler(IFaqRepository faqRepository, IMapper mapper)
        {
            _faqRepository = faqRepository;
            _mapper = mapper;
        }
        public async Task<BaseCommonResponse> Handle(CreateFaqCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommonResponse();
            var validator = new FaqCreateDtoValidator();
            var validationResult = await validator.ValidateAsync(request.FaqFormDto);
            var FaqDto = _mapper.Map<FaqDto>(request.FaqFormDto);
            if (validationResult.IsValid == false)
            {
                response.Success = false;
                response.Message = "Creation Faild";
                response.Errors = validationResult.Errors.Select(x => x.ErrorMessage).ToList();
                response.Status = "400";
            }
            else
            {
                string faqId;
                bool flag = true;

                while (true)
                {
                    faqId = Guid.NewGuid().ToString();
                    flag = await _faqRepository.Exists(faqId);
                    if (flag == false)
                    {
                        FaqDto.Id = faqId;
                        break;
                    }
                }

                var data = _mapper.Map<Faq>(FaqDto);

                var saveData = await _faqRepository.Add(data);

                response.Data = _mapper.Map<FaqDto>(saveData);
                response.Success = true;
                response.Message = "Created Successfully";
                response.Status = "200";
                    
            }
            return response;
        }
    }
}
