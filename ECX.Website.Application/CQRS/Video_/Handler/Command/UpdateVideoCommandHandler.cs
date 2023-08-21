using AutoMapper;
using ECX.Website.Application.Contracts.Persistence;
using ECX.Website.Application.CQRS.Video_.Request.Command;
using ECX.Website.Application.DTOs.Video;
using ECX.Website.Application.DTOs.Video.Validators;
using ECX.Website.Application.DTOs.Common.Validators;
using ECX.Website.Application.Exceptions;
using ECX.Website.Application.Response;
using ECX.Website.Domain;
using MediatR;

namespace ECX.Website.Application.CQRS.Video_.Handler.Command
{
    public class UpdateVideoCommandHandler : IRequestHandler<UpdateVideoCommand, BaseCommonResponse>
    {
        private IVideoRepository _videoRepository;
        private IMapper _mapper;
        public UpdateVideoCommandHandler(IVideoRepository videoRepository, IMapper mapper)
        {
            _videoRepository = videoRepository;
            _mapper = mapper;
        }

        public async Task<BaseCommonResponse> Handle(UpdateVideoCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommonResponse();
            var validator = new VideoUpdateDtoValidator();
            var validationResult = await validator.ValidateAsync(request.VideoFormDto);
            var VideoDto = _mapper.Map<VideoDto>(request.VideoFormDto);
            var flag = await _videoRepository.Exists(request.VideoFormDto.Id);

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
                            nameof(Video), request.VideoFormDto.Id).Message.ToString();
                response.Status = "404";
            }
            else 
            {
                if (request.VideoFormDto.ImgFile != null)
                {
                    try
                    {
                        var imageValidator = new ImageValidator();
                        var imgValidationResult = await imageValidator.ValidateAsync(request.VideoFormDto.ImgFile);

                        if (imgValidationResult.IsValid == false)
                        {
                            response.Success = false;
                            response.Message = "Update Failed";
                            response.Errors = imgValidationResult.Errors.Select(x => x.ErrorMessage).ToList();
                            response.Status = "400";
                        }
                        else
                        {
                            var oldImage = (await _videoRepository.GetById(
                                request.VideoFormDto.Id)).ImgName;
                            

                            string oldPath = Path.Combine(
                                Directory.GetCurrentDirectory(), @"wwwroot\image",oldImage);
                            File.Delete(oldPath);

                            string contentType = request.VideoFormDto.ImgFile.ContentType.ToString();
                            string ext = contentType.Split('/')[1];
                            string fileName = Guid.NewGuid().ToString() + "." + ext;
                            string path = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\image", fileName);

                            using (Stream stream = new FileStream(path, FileMode.Create))
                            {
                                request.VideoFormDto.ImgFile.CopyTo(stream);
                            }
                           
                            VideoDto.ImgName = fileName;
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
                    VideoDto.ImgName = (await _videoRepository.GetById(
                                request.VideoFormDto.Id)).ImgName;
                } 

                var updateData = await _videoRepository.GetById(request.VideoFormDto.Id);
                
                _mapper.Map(VideoDto, updateData);

                var data = await _videoRepository.Update(updateData);

                response.Data = _mapper.Map<VideoDto>(data);
                response.Success = true;
                response.Message = "Updated Successfull";
                response.Status = "200";
            }
            return response;
        }
    }
 }

