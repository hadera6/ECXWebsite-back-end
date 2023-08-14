using AutoMapper;
using ECX.Website.Application.Contracts.Persistence;
using ECX.Website.Application.CQRS.Commodity_.Request.Command;
using ECX.Website.Application.DTOs.Commodity;
using ECX.Website.Application.DTOs.Commodity.Validators;
using ECX.Website.Application.DTOs.Common.Validators;
using ECX.Website.Application.Exceptions;
using ECX.Website.Application.Response;
using ECX.Website.Domain;
using MediatR;

namespace ECX.Website.Application.CQRS.Commodity_.Handler.Command
{
    public class UpdateCommodityCommandHandler : IRequestHandler<UpdateCommodityCommand, BaseCommonResponse>
    {
        private ICommodityRepository _commodityRepository;
        private IMapper _mapper;
        public UpdateCommodityCommandHandler(ICommodityRepository commodityRepository, IMapper mapper)
        {
            _commodityRepository = commodityRepository;
            _mapper = mapper;
        }

        public async Task<BaseCommonResponse> Handle(UpdateCommodityCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommonResponse();
            var validator = new CommodityUpdateDtoValidator();
            var validationResult = await validator.ValidateAsync(request.CommodityFormDto);
            var CommodityDto = _mapper.Map<CommodityDto>(request.CommodityFormDto);
            var flag = await _commodityRepository.Exists(request.CommodityFormDto.Id);

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
                            nameof(Commodity), request.CommodityFormDto.Id).Message.ToString();
                response.Status = "404";
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
                            response.Status = "400";
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
                        response.Success = false;
                        response.Message = "Update Failed";
                        response.Errors = new List<string> { ex.Message };
                        response.Status = "400";
                    }
                }
                else
                {
                    CommodityDto.ImgName = (await _commodityRepository.GetById(
                                request.CommodityFormDto.Id)).ImgName;
                } 

                var updateData = await _commodityRepository.GetById(request.CommodityFormDto.Id);
                
                _mapper.Map(CommodityDto, updateData);

                var data = await _commodityRepository.Update(updateData);

                response.Data = _mapper.Map<CommodityDto>(data);
                response.Success = true;
                response.Message = "Updated Successfull";
                response.Status = "200";
            }
            return response;
        }
    }
 }

