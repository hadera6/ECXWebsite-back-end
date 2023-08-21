using AutoMapper;
using ECX.Website.Application.Contracts.Persistence;
using ECX.Website.Application.CQRS.Language_.Request.Command;
using ECX.Website.Application.DTOs.Language;
using ECX.Website.Application.DTOs.Language.Validators;
using ECX.Website.Application.DTOs.Common.Validators;
using ECX.Website.Application.Exceptions;
using ECX.Website.Application.Response;
using ECX.Website.Domain;
using MediatR;

namespace ECX.Website.Application.CQRS.Language_.Handler.Command
{
    public class UpdateLanguageCommandHandler : IRequestHandler<UpdateLanguageCommand, BaseCommonResponse>
    {
        private ILanguageRepository _languageRepository;
        private IMapper _mapper;
        public UpdateLanguageCommandHandler(ILanguageRepository languageRepository, IMapper mapper)
        {
            _languageRepository = languageRepository;
            _mapper = mapper;
        }

        public async Task<BaseCommonResponse> Handle(UpdateLanguageCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommonResponse();
            var validator = new LanguageUpdateDtoValidator();
            var validationResult = await validator.ValidateAsync(request.LanguageFormDto);
            var LanguageDto = _mapper.Map<LanguageDto>(request.LanguageFormDto);
            var flag = await _languageRepository.Exists(request.LanguageFormDto.Id);

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
                            nameof(Language), request.LanguageFormDto.Id).Message.ToString();
                response.Status = "404";
            }
            else 
            {
                if (request.LanguageFormDto.ImgFile != null)
                {
                    try
                    {
                        var imageValidator = new ImageValidator();
                        var imgValidationResult = await imageValidator.ValidateAsync(request.LanguageFormDto.ImgFile);

                        if (imgValidationResult.IsValid == false)
                        {
                            response.Success = false;
                            response.Message = "Update Failed";
                            response.Errors = imgValidationResult.Errors.Select(x => x.ErrorMessage).ToList();
                            response.Status = "400";
                        }
                        else
                        {
                            var oldImage = (await _languageRepository.GetById(
                                request.LanguageFormDto.Id)).ImgName;
                            

                            string oldPath = Path.Combine(
                                Directory.GetCurrentDirectory(), @"wwwroot\image",oldImage);
                            File.Delete(oldPath);

                            string contentType = request.LanguageFormDto.ImgFile.ContentType.ToString();
                            string ext = contentType.Split('/')[1];
                            string fileName = Guid.NewGuid().ToString() + "." + ext;
                            string path = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\image", fileName);

                            using (Stream stream = new FileStream(path, FileMode.Create))
                            {
                                request.LanguageFormDto.ImgFile.CopyTo(stream);
                            }
                           
                            LanguageDto.ImgName = fileName;
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
                    LanguageDto.ImgName = (await _languageRepository.GetById(
                                request.LanguageFormDto.Id)).ImgName;
                } 

                var updateData = await _languageRepository.GetById(request.LanguageFormDto.Id);
                
                _mapper.Map(LanguageDto, updateData);

                var data = await _languageRepository.Update(updateData);

                response.Data = _mapper.Map<LanguageDto>(data);
                response.Success = true;
                response.Message = "Updated Successfull";
                response.Status = "200";
            }
            return response;
        }
    }
 }

