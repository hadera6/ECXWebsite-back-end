using AutoMapper;
using ECX.Website.Application.Contracts.Persistence;
using ECX.Website.Application.CQRS.ExternalLink_.Request.Command;
using ECX.Website.Application.DTOs.ExternalLink;
using ECX.Website.Application.DTOs.ExternalLink.Validators;
using ECX.Website.Application.DTOs.Common.Validators;
using ECX.Website.Application.Exceptions;
using ECX.Website.Application.Response;
using ECX.Website.Domain;
using MediatR;

namespace ECX.Website.Application.CQRS.ExternalLink_.Handler.Command
{
    public class UpdateExternalLinkCommandHandler : IRequestHandler<UpdateExternalLinkCommand, BaseCommonResponse>
    {
        private IExternalLinkRepository _externalLinkRepository;
        private IMapper _mapper;
        public UpdateExternalLinkCommandHandler(IExternalLinkRepository externalLinkRepository, IMapper mapper)
        {
            _externalLinkRepository = externalLinkRepository;
            _mapper = mapper;
        }

        public async Task<BaseCommonResponse> Handle(UpdateExternalLinkCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommonResponse();
            var validator = new ExternalLinkUpdateDtoValidator();
            var validationResult = await validator.ValidateAsync(request.ExternalLinkFormDto);
            var ExternalLinkDto = _mapper.Map<ExternalLinkDto>(request.ExternalLinkFormDto);
            var flag = await _externalLinkRepository.Exists(request.ExternalLinkFormDto.Id);

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
                            nameof(ExternalLink), request.ExternalLinkFormDto.Id).Message.ToString();
                response.Status = "404";
            }
            else 
            {
                if (request.ExternalLinkFormDto.ImgFile != null)
                {
                    try
                    {
                        var imageValidator = new ImageValidator();
                        var imgValidationResult = await imageValidator.ValidateAsync(request.ExternalLinkFormDto.ImgFile);

                        if (imgValidationResult.IsValid == false)
                        {
                            response.Success = false;
                            response.Message = "Update Failed";
                            response.Errors = imgValidationResult.Errors.Select(x => x.ErrorMessage).ToList();
                            response.Status = "400";
                        }
                        else
                        {
                            var oldImage = (await _externalLinkRepository.GetById(
                                request.ExternalLinkFormDto.Id)).ImgName;
                            

                            string oldPath = Path.Combine(
                                Directory.GetCurrentDirectory(), @"wwwroot\image",oldImage);
                            File.Delete(oldPath);

                            string contentType = request.ExternalLinkFormDto.ImgFile.ContentType.ToString();
                            string ext = contentType.Split('/')[1];
                            string fileName = Guid.NewGuid().ToString() + "." + ext;
                            string path = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\image", fileName);

                            using (Stream stream = new FileStream(path, FileMode.Create))
                            {
                                request.ExternalLinkFormDto.ImgFile.CopyTo(stream);
                            }
                           
                            ExternalLinkDto.ImgName = fileName;
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
                    ExternalLinkDto.ImgName = (await _externalLinkRepository.GetById(
                                request.ExternalLinkFormDto.Id)).ImgName;
                } 

                var updateData = await _externalLinkRepository.GetById(request.ExternalLinkFormDto.Id);
                
                _mapper.Map(ExternalLinkDto, updateData);

                var data = await _externalLinkRepository.Update(updateData);

                response.Data = _mapper.Map<ExternalLinkDto>(data);
                response.Success = true;
                response.Message = "Updated Successfull";
                response.Status = "200";
            }
            return response;
        }
    }
 }

