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

            if (validationResult.IsValid == false)
            {
                response.Success = false;
                response.Message = "Creation Faild";
                response.Errors = validationResult.Errors.Select(x => x.ErrorMessage).ToList();
                response.Status = "400";
            }
            else
            {
                try
                {
                    var imageValidator = new ImageValidator();
                    var imgValidationResult = await imageValidator.ValidateAsync(request.SubscriptionFormDto.ImgFile);

                    if (imgValidationResult.IsValid == false)
                    {
                        response.Success = false;
                        response.Message = "Creation Faild";
                        response.Errors = imgValidationResult.Errors.Select(x => x.ErrorMessage).ToList();
                        response.Status = "400";
                    }
                    else
                    {
                        string contentType = request.SubscriptionFormDto.ImgFile.ContentType.ToString();
                        string ext = contentType.Split('/')[1];
                        string fileName = Guid.NewGuid().ToString() + "." + ext;
                        string path = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\image", fileName);

                        using (Stream stream = new FileStream(path, FileMode.Create))
                        {
                            request.SubscriptionFormDto.ImgFile.CopyTo(stream);
                        }
                        var SubscriptionDto = _mapper.Map<SubscriptionDto>(request.SubscriptionFormDto);
                        SubscriptionDto.ImgName = fileName;

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
                }
                catch (Exception ex)
                {
                    response.Success = false;
                    response.Message = "Creation Failed";
                    response.Errors = new List<string> { ex.Message };
                    response.Status = "400";
                }
            }
            return response;
        }
    }
}
