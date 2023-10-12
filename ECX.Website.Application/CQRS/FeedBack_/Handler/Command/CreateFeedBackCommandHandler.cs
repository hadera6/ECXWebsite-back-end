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
using System.IO;
using ECX.Website.Application.DTOs.Common.Validators;

namespace ECX.Website.Application.CQRS.FeedBack_.Handler.Command
{
    public class CreateFeedBackCommandHandler : IRequestHandler<CreateFeedBackCommand, BaseCommonResponse>
    {
        private IFeedBackRepository _feedBackRepository;
        private IMapper _mapper;

        public CreateFeedBackCommandHandler(IFeedBackRepository feedBackRepository, IMapper mapper)
        {
            _feedBackRepository = feedBackRepository;
            _mapper = mapper;
        }
        public async Task<BaseCommonResponse> Handle(CreateFeedBackCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommonResponse();
            var validator = new FeedBackCreateDtoValidator();
            var validationResult = await validator.ValidateAsync(request.FeedBackFormDto);
            var FeedBackDto = _mapper.Map<FeedBackDto>(request.FeedBackFormDto);
            if (validationResult.IsValid == false)
            {
                response.Success = false;
                response.Message = "Creation Faild";
                response.Errors = validationResult.Errors.Select(x => x.ErrorMessage).ToList();
                response.Status = "400";
            }
            else
            {
                string FeedBackId;
                bool flag = true;

                while (true)
                {
                    FeedBackId = Guid.NewGuid().ToString();
                    flag = await _feedBackRepository.Exists(FeedBackId);
                    if (flag == false)
                    {
                        FeedBackDto.Id = FeedBackId;
                        break;
                    }
                }

                var data = _mapper.Map<FeedBack>(FeedBackDto);

                var saveData = await _feedBackRepository.Add(data);

                response.Data = _mapper.Map<FeedBackDto>(saveData);
                response.Success = true;
                response.Message = "Created Successfully";
                response.Status = "200";

            }
            return response;
        }
    }
}
