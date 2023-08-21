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
                    var imgValidationResult = await imageValidator.ValidateAsync(request.FaqFormDto.ImgFile);

                    if (imgValidationResult.IsValid == false)
                    {
                        response.Success = false;
                        response.Message = "Creation Faild";
                        response.Errors = imgValidationResult.Errors.Select(x => x.ErrorMessage).ToList();
                        response.Status = "400";
                    }
                    else
                    {
                        string contentType = request.FaqFormDto.ImgFile.ContentType.ToString();
                        string ext = contentType.Split('/')[1];
                        string fileName = Guid.NewGuid().ToString() + "." + ext;
                        string path = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\image", fileName);

                        using (Stream stream = new FileStream(path, FileMode.Create))
                        {
                            request.FaqFormDto.ImgFile.CopyTo(stream);
                        }
                        var FaqDto = _mapper.Map<FaqDto>(request.FaqFormDto);
                        FaqDto.ImgName = fileName;

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
