using AutoMapper;
using ECX.Website.Application.Contracts.Persistence;
using ECX.Website.Application.CQRS.Brochure_.Request.Command;
using ECX.Website.Application.DTOs.Brochure;
using ECX.Website.Application.DTOs.Brochure.Validators;
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

namespace ECX.Website.Application.CQRS.Brochure_.Handler.Command
{
    public class CreateBrochureCommandHandler : IRequestHandler<CreateBrochureCommand, BaseCommonResponse>
    {
        private IBrochureRepository _brochureRepository;
        private IMapper _mapper;

        public CreateBrochureCommandHandler(IBrochureRepository brochureRepository, IMapper mapper)
        {
            _brochureRepository = brochureRepository;
            _mapper = mapper;
        }
        public async Task<BaseCommonResponse> Handle(CreateBrochureCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommonResponse();
            var validator = new BrochureCreateDtoValidator();
            var validationResult = await validator.ValidateAsync(request.BrochureFormDto);

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
                    var imgValidationResult = await imageValidator.ValidateAsync(request.BrochureFormDto.ImgFile);

                    if (imgValidationResult.IsValid == false)
                    {
                        response.Success = false;
                        response.Message = "Creation Faild";
                        response.Errors = imgValidationResult.Errors.Select(x => x.ErrorMessage).ToList();
                        response.Status = "400";
                    }
                    else
                    {
                        string contentType = request.BrochureFormDto.ImgFile.ContentType.ToString();
                        string ext = contentType.Split('/')[1];
                        string fileName = Guid.NewGuid().ToString() + "." + ext;
                        string path = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\image", fileName);

                        using (Stream stream = new FileStream(path, FileMode.Create))
                        {
                            request.BrochureFormDto.ImgFile.CopyTo(stream);
                        }
                        var BrochureDto = _mapper.Map<BrochureDto>(request.BrochureFormDto);
                        BrochureDto.ImgName = fileName;

                        string brochureId;
                        bool flag = true;

                        while (true)
                        {
                            brochureId = Guid.NewGuid().ToString();
                            flag = await _brochureRepository.Exists(brochureId);
                            if (flag == false)
                            {
                                BrochureDto.Id = brochureId;
                                break;
                            }
                        }

                        var data = _mapper.Map<Brochure>(BrochureDto);

                        var saveData = await _brochureRepository.Add(data);

                        response.Data = _mapper.Map<BrochureDto>(saveData);
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
