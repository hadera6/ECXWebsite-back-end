using AutoMapper;
using ECX.Website.Application.Contracts.Persistence;
using ECX.Website.Application.CQRS.TradingCenter_.Request.Command;
using ECX.Website.Application.DTOs.TradingCenter;
using ECX.Website.Application.DTOs.TradingCenter.Validators;
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

namespace ECX.Website.Application.CQRS.TradingCenter_.Handler.Command
{
    public class CreateTradingCenterCommandHandler : IRequestHandler<CreateTradingCenterCommand, BaseCommonResponse>
    {
        private ITradingCenterRepository _tradingCenterRepository;
        private IMapper _mapper;

        public CreateTradingCenterCommandHandler(ITradingCenterRepository tradingCenterRepository, IMapper mapper)
        {
            _tradingCenterRepository = tradingCenterRepository;
            _mapper = mapper;
        }
        public async Task<BaseCommonResponse> Handle(CreateTradingCenterCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommonResponse();
            var validator = new TradingCenterCreateDtoValidator();
            var validationResult = await validator.ValidateAsync(request.TradingCenterFormDto);

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
                    var imgValidationResult = await imageValidator.ValidateAsync(request.TradingCenterFormDto.ImgFile);

                    if (imgValidationResult.IsValid == false)
                    {
                        response.Success = false;
                        response.Message = "Creation Faild";
                        response.Errors = imgValidationResult.Errors.Select(x => x.ErrorMessage).ToList();
                        response.Status = "400";
                    }
                    else
                    {
                        string contentType = request.TradingCenterFormDto.ImgFile.ContentType.ToString();
                        string ext = contentType.Split('/')[1];
                        string fileName = Guid.NewGuid().ToString() + "." + ext;
                        string path = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\image", fileName);

                        using (Stream stream = new FileStream(path, FileMode.Create))
                        {
                            request.TradingCenterFormDto.ImgFile.CopyTo(stream);
                        }
                        var TradingCenterDto = _mapper.Map<TradingCenterDto>(request.TradingCenterFormDto);
                        TradingCenterDto.ImgName = fileName;

                        string tradingCenterId;
                        bool flag = true;

                        while (true)
                        {
                            tradingCenterId = Guid.NewGuid().ToString();
                            flag = await _tradingCenterRepository.Exists(tradingCenterId);
                            if (flag == false)
                            {
                                TradingCenterDto.Id = tradingCenterId;
                                break;
                            }
                        }

                        var data = _mapper.Map<TradingCenter>(TradingCenterDto);

                        var saveData = await _tradingCenterRepository.Add(data);

                        response.Data = _mapper.Map<TradingCenterDto>(saveData);
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
