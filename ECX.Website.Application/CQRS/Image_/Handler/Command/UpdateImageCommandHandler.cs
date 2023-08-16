using AutoMapper;
using ECX.Website.Application.Contracts.Persistence;
using ECX.Website.Application.CQRS.Image_.Request.Command;
using ECX.Website.Application.DTOs.Image;
using ECX.Website.Application.DTOs.Image.Validators;
using ECX.Website.Application.DTOs.Common.Validators;
using ECX.Website.Application.Exceptions;
using ECX.Website.Application.Response;
using ECX.Website.Domain;
using MediatR;

namespace ECX.Website.Application.CQRS.Image_.Handler.Command
{
    public class UpdateImageCommandHandler : IRequestHandler<UpdateImageCommand, BaseCommonResponse>
    {
        private IImageRepository _imageRepository;
        private IMapper _mapper;
        public UpdateImageCommandHandler(IImageRepository imageRepository, IMapper mapper)
        {
            _imageRepository = imageRepository;
            _mapper = mapper;
        }

        public async Task<BaseCommonResponse> Handle(UpdateImageCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommonResponse();
            var validator = new ImageUpdateDtoValidator();
            var validationResult = await validator.ValidateAsync(request.ImageFormDto);
            var ImageDto = _mapper.Map<ImageDto>(request.ImageFormDto);
            var flag = await _imageRepository.Exists(request.ImageFormDto.Id);

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
                            nameof(Image), request.ImageFormDto.Id).Message.ToString();
                response.Status = "404";
            }
            else 
            {
                if (request.ImageFormDto.ImgFile != null)
                {
                    try
                    {
                        var imageValidator = new ImageValidator();
                        var imgValidationResult = await imageValidator.ValidateAsync(request.ImageFormDto.ImgFile);

                        if (imgValidationResult.IsValid == false)
                        {
                            response.Success = false;
                            response.Message = "Update Failed";
                            response.Errors = imgValidationResult.Errors.Select(x => x.ErrorMessage).ToList();
                            response.Status = "400";
                        }
                        else
                        {
                            var oldImage = (await _imageRepository.GetById(
                                request.ImageFormDto.Id)).ImgName;
                            

                            string oldPath = Path.Combine(
                                Directory.GetCurrentDirectory(), @"wwwroot\image",oldImage);
                            File.Delete(oldPath);

                            string contentType = request.ImageFormDto.ImgFile.ContentType.ToString();
                            string ext = contentType.Split('/')[1];
                            string fileName = Guid.NewGuid().ToString() + "." + ext;
                            string path = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\image", fileName);

                            using (Stream stream = new FileStream(path, FileMode.Create))
                            {
                                request.ImageFormDto.ImgFile.CopyTo(stream);
                            }
                           
                            ImageDto.ImgName = fileName;
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
                    ImageDto.ImgName = (await _imageRepository.GetById(
                                request.ImageFormDto.Id)).ImgName;
                } 

                var updateData = await _imageRepository.GetById(request.ImageFormDto.Id);
                
                _mapper.Map(ImageDto, updateData);

                var data = await _imageRepository.Update(updateData);

                response.Data = _mapper.Map<ImageDto>(data);
                response.Success = true;
                response.Message = "Updated Successfull";
                response.Status = "200";
            }
            return response;
        }
    }
 }

