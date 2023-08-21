using AutoMapper;
using ECX.Website.Application.Contracts.Persistence;
using ECX.Website.Application.CQRS.TradingCenter_.Request.Command;
using ECX.Website.Application.DTOs.TradingCenter;
using ECX.Website.Application.DTOs.TradingCenter.Validators;
using ECX.Website.Application.DTOs.Common.Validators;
using ECX.Website.Application.Exceptions;
using ECX.Website.Application.Response;
using ECX.Website.Domain;
using MediatR;

namespace ECX.Website.Application.CQRS.TradingCenter_.Handler.Command
{
    public class UpdateTradingCenterCommandHandler : IRequestHandler<UpdateTradingCenterCommand, BaseCommonResponse>
    {
        private ITradingCenterRepository _tradingCenterRepository;
        private IMapper _mapper;
        public UpdateTradingCenterCommandHandler(ITradingCenterRepository tradingCenterRepository, IMapper mapper)
        {
            _tradingCenterRepository = tradingCenterRepository;
            _mapper = mapper;
        }

        public async Task<BaseCommonResponse> Handle(UpdateTradingCenterCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommonResponse();
            var validator = new TradingCenterUpdateDtoValidator();
            var validationResult = await validator.ValidateAsync(request.TradingCenterFormDto);
            var TradingCenterDto = _mapper.Map<TradingCenterDto>(request.TradingCenterFormDto);
            var flag = await _tradingCenterRepository.Exists(request.TradingCenterFormDto.Id);

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
                            nameof(TradingCenter), request.TradingCenterFormDto.Id).Message.ToString();
                response.Status = "404";
            }
            else 
            {
                if (request.TradingCenterFormDto.ImgFile != null)
                {
                    try
                    {
                        var imageValidator = new ImageValidator();
                        var imgValidationResult = await imageValidator.ValidateAsync(request.TradingCenterFormDto.ImgFile);

                        if (imgValidationResult.IsValid == false)
                        {
                            response.Success = false;
                            response.Message = "Update Failed";
                            response.Errors = imgValidationResult.Errors.Select(x => x.ErrorMessage).ToList();
                            response.Status = "400";
                        }
                        else
                        {
                            var oldImage = (await _tradingCenterRepository.GetById(
                                request.TradingCenterFormDto.Id)).ImgName;
                            

                            string oldPath = Path.Combine(
                                Directory.GetCurrentDirectory(), @"wwwroot\image",oldImage);
                            File.Delete(oldPath);

                            string contentType = request.TradingCenterFormDto.ImgFile.ContentType.ToString();
                            string ext = contentType.Split('/')[1];
                            string fileName = Guid.NewGuid().ToString() + "." + ext;
                            string path = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\image", fileName);

                            using (Stream stream = new FileStream(path, FileMode.Create))
                            {
                                request.TradingCenterFormDto.ImgFile.CopyTo(stream);
                            }
                           
                            TradingCenterDto.ImgName = fileName;
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
                    TradingCenterDto.ImgName = (await _tradingCenterRepository.GetById(
                                request.TradingCenterFormDto.Id)).ImgName;
                } 

                var updateData = await _tradingCenterRepository.GetById(request.TradingCenterFormDto.Id);
                
                _mapper.Map(TradingCenterDto, updateData);

                var data = await _tradingCenterRepository.Update(updateData);

                response.Data = _mapper.Map<TradingCenterDto>(data);
                response.Success = true;
                response.Message = "Updated Successfull";
                response.Status = "200";
            }
            return response;
        }
    }
 }

