using AutoMapper;
using ECX.Website.Application.Contracts.Persistence;
using ECX.Website.Application.CQRS.Commodities.Request.Command;
using ECX.Website.Application.DTOs.Commodity;
using ECX.Website.Application.DTOs.Commodity.Validators;
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

namespace ECX.Website.Application.CQRS.Commodities.Handler.Command
{
    public class CreateCommodityCommandHandler : IRequestHandler<CreateCommodityCommand, BaseCommonResponse>
    {
        BaseCommonResponse response;
        private ICommodityRepository _commodityRepository;
        private IMapper _mapper;
        
        public CreateCommodityCommandHandler(ICommodityRepository commodityRepository, IMapper mapper)
        {
            _commodityRepository = commodityRepository;
            _mapper = mapper;
        }
        public async Task<BaseCommonResponse> Handle(CreateCommodityCommand request, CancellationToken cancellationToken)
        {
            response = new BaseCommonResponse();
            var validator = new CommodityCreateDtoValidator();
            var validationResult = await validator.ValidateAsync(request.CommodityFormDto);

            if (validationResult.IsValid == false)
            {
                response.Success = false;
                response.Message = "Creation Faild";
                response.Errors = validationResult.Errors.Select(x => x.ErrorMessage).ToList();
            }
            else
            {
                try
                {
                    var imageValidator = new ImageValidator();
                    var imgValidationResult = await imageValidator.ValidateAsync(request.CommodityFormDto.ImgFile);

                    if (imgValidationResult.IsValid == false)
                    {
                        response.Success = false;
                        response.Message = "Creation Faild";
                        response.Errors = imgValidationResult.Errors.Select(x => x.ErrorMessage).ToList();
                    }
                    else
                    {
                        string contentType = request.CommodityFormDto.ImgFile.ContentType.ToString();
                        string ext = contentType.Split('/')[1];
                        string fileName = Guid.NewGuid().ToString() +"."+ext;
                        string path = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\image", fileName);

                        using (Stream stream = new FileStream(path, FileMode.Create))
                        {
                            request.CommodityFormDto.ImgFile.CopyTo(stream);
                        }
                        var CommodityDto = _mapper.Map<CommodityDto>(request.CommodityFormDto);
                        CommodityDto.ImgName = fileName;

                        var commodity = _mapper.Map<Commodity>(CommodityDto);
                        
                        var data = await _commodityRepository.Add(commodity);
                        response.Success = true;
                        response.Message = "Creation Successfull";
                    }    
                }
                catch (Exception ex)
                {

                }
            }
            return response;
        }
    }
}
