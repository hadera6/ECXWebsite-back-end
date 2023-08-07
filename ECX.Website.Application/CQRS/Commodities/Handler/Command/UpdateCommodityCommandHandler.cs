using AutoMapper;
using ECX.Website.Application.CQRS.Commodities.Request.Command;
using ECX.Website.Application.Exceptions;
using ECX.Website.Application.Contracts.Persistence;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ECX.Website.Domain;
using ECX.Website.Application.DTOs.Commodity.Validators;
using ECX.Website.Application.Response;
using ECX.Website.Application.DTOs.Common.Validators;
using ECX.Website.Application.DTOs.Commodity;

namespace ECX.Website.Application.CQRS.Commodities.Handler.Command
{
    public class UpdateCommodityCommandHandler : IRequestHandler<UpdateCommodityCommand, BaseCommonResponse>
    {
        private BaseCommonResponse response;
        private ICommodityRepository _commodityRepository;
        private IMapper _mapper;
        public UpdateCommodityCommandHandler(ICommodityRepository commodityRepository, IMapper mapper)
        {
            _commodityRepository = commodityRepository;
            _mapper = mapper;
        }

        public async Task<BaseCommonResponse> Handle(UpdateCommodityCommand request, CancellationToken cancellationToken)
        {
            //var validator = new CommodityUpdateDtoValidator();
            //var validationResult = await validator.ValidateAsync(request.CommodityFormDto);
            //if (validationResult.IsValid == false)
            //  throw new ValidationException(validationResult);
            
            response = new BaseCommonResponse();
            var validator = new CommodityUpdateDtoValidator();
            var validationResult = await validator.ValidateAsync(request.CommodityFormDto);
            var CommodityDto = _mapper.Map<CommodityDto>(request.CommodityFormDto);

            if (validationResult.IsValid == false)
            {
                response.Success = false;
                response.Message = "Update Failed";
                response.Errors = validationResult.Errors.Select(x => x.ErrorMessage).ToList();
            }
            else 
            {

                if (request.CommodityFormDto.ImgFile != null)
                {
                    try
                    {
                        var imageValidator = new ImageValidator();
                        var imgValidationResult = await imageValidator.ValidateAsync(request.CommodityFormDto.ImgFile);

                        if (imgValidationResult.IsValid == false)
                        {
                            response.Success = false;
                            response.Message = "Update Failed";
                            response.Errors = imgValidationResult.Errors.Select(x => x.ErrorMessage).ToList();
                        }
                        else
                        {
                            var oldImage = (await _commodityRepository.GetById(
                                request.CommodityFormDto.Id)).ImgName;
                            

                            string oldPath = Path.Combine(
                                Directory.GetCurrentDirectory(), @"wwwroot\image",oldImage);
                            File.Delete(oldPath);

                            string contentType = request.CommodityFormDto.ImgFile.ContentType.ToString();
                            string ext = contentType.Split('/')[1];
                            string fileName = Guid.NewGuid().ToString() + "." + ext;
                            string path = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\image", fileName);

                            using (Stream stream = new FileStream(path, FileMode.Create))
                            {
                                request.CommodityFormDto.ImgFile.CopyTo(stream);
                            }
                           
                            CommodityDto.ImgName = fileName;
                        }
                    }
                    catch (Exception ex)
                    {
                        
                    }
                }
                else
                {
                    CommodityDto.ImgName = (await _commodityRepository.GetById(
                                request.CommodityFormDto.Id)).ImgName;
                } 


                var commodity = await _commodityRepository.GetById(request.CommodityFormDto.Id);
                
                _mapper.Map(CommodityDto, commodity);
                await _commodityRepository.Update(commodity);
                response.Success = true;
                response.Message = "Updated Successfull";
            }
            return response;
        }
    }
 }

