using AutoMapper;
using ECX.Website.Application.Contracts.Persistence;
using ECX.Website.Application.CQRS.WareHouse_.Request.Command;
using ECX.Website.Application.DTOs.WareHouse;
using ECX.Website.Application.DTOs.WareHouse.Validators;
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

namespace ECX.Website.Application.CQRS.WareHouse_.Handler.Command
{
    public class CreateWareHouseCommandHandler : IRequestHandler<CreateWareHouseCommand, BaseCommonResponse>
    {
        private IWareHouseRepository _wareHouseRepository;
        private IMapper _mapper;

        public CreateWareHouseCommandHandler(IWareHouseRepository wareHouseRepository, IMapper mapper)
        {
            _wareHouseRepository = wareHouseRepository;
            _mapper = mapper;
        }
        public async Task<BaseCommonResponse> Handle(CreateWareHouseCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommonResponse();
            var validator = new WareHouseCreateDtoValidator();
            var validationResult = await validator.ValidateAsync(request.WareHouseFormDto);

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
                    var imgValidationResult = await imageValidator.ValidateAsync(request.WareHouseFormDto.ImgFile);

                    if (imgValidationResult.IsValid == false)
                    {
                        response.Success = false;
                        response.Message = "Creation Faild";
                        response.Errors = imgValidationResult.Errors.Select(x => x.ErrorMessage).ToList();
                        response.Status = "400";
                    }
                    else
                    {
                        string contentType = request.WareHouseFormDto.ImgFile.ContentType.ToString();
                        string ext = contentType.Split('/')[1];
                        string fileName = Guid.NewGuid().ToString() + "." + ext;
                        string path = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\image", fileName);

                        using (Stream stream = new FileStream(path, FileMode.Create))
                        {
                            request.WareHouseFormDto.ImgFile.CopyTo(stream);
                        }
                        var WareHouseDto = _mapper.Map<WareHouseDto>(request.WareHouseFormDto);
                        WareHouseDto.ImgName = fileName;

                        string wareHouseId;
                        bool flag = true;

                        while (true)
                        {
                            wareHouseId = Guid.NewGuid().ToString();
                            flag = await _wareHouseRepository.Exists(wareHouseId);
                            if (flag == false)
                            {
                                WareHouseDto.Id = wareHouseId;
                                break;
                            }
                        }

                        var data = _mapper.Map<WareHouse>(WareHouseDto);

                        var saveData = await _wareHouseRepository.Add(data);

                        response.Data = _mapper.Map<WareHouseDto>(saveData);
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
