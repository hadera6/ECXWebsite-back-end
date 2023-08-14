using AutoMapper;
using ECX.Website.Application.Contracts.Persistence;
using ECX.Website.Application.CQRS.News_.Request.Command;
using ECX.Website.Application.DTOs.News;
using ECX.Website.Application.DTOs.News.Validators;
using ECX.Website.Application.DTOs.Common.Validators;
using ECX.Website.Application.Exceptions;
using ECX.Website.Application.Response;
using ECX.Website.Domain;
using MediatR;

namespace ECX.Website.Application.CQRS.News_.Handler.Command
{
    public class UpdateNewsCommandHandler : IRequestHandler<UpdateNewsCommand, BaseCommonResponse>
    {
        private INewsRepository _newsRepository;
        private IMapper _mapper;
        public UpdateNewsCommandHandler(INewsRepository newsRepository, IMapper mapper)
        {
            _newsRepository = newsRepository;
            _mapper = mapper;
        }

        public async Task<BaseCommonResponse> Handle(UpdateNewsCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommonResponse();
            var validator = new NewsUpdateDtoValidator();
            var validationResult = await validator.ValidateAsync(request.NewsFormDto);
            var NewsDto = _mapper.Map<NewsDto>(request.NewsFormDto);
            var flag = await _newsRepository.Exists(request.NewsFormDto.Id);

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
                            nameof(News), request.NewsFormDto.Id).Message.ToString();
                response.Status = "404";
            }
            else 
            {
                if (request.NewsFormDto.ImgFile != null)
                {
                    try
                    {
                        var imageValidator = new ImageValidator();
                        var imgValidationResult = await imageValidator.ValidateAsync(request.NewsFormDto.ImgFile);

                        if (imgValidationResult.IsValid == false)
                        {
                            response.Success = false;
                            response.Message = "Update Failed";
                            response.Errors = imgValidationResult.Errors.Select(x => x.ErrorMessage).ToList();
                            response.Status = "400";
                        }
                        else
                        {
                            var oldImage = (await _newsRepository.GetById(
                                request.NewsFormDto.Id)).ImgName;
                            

                            string oldPath = Path.Combine(
                                Directory.GetCurrentDirectory(), @"wwwroot\image",oldImage);
                            File.Delete(oldPath);

                            string contentType = request.NewsFormDto.ImgFile.ContentType.ToString();
                            string ext = contentType.Split('/')[1];
                            string fileName = Guid.NewGuid().ToString() + "." + ext;
                            string path = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\image", fileName);

                            using (Stream stream = new FileStream(path, FileMode.Create))
                            {
                                request.NewsFormDto.ImgFile.CopyTo(stream);
                            }
                           
                            NewsDto.ImgName = fileName;
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
                    NewsDto.ImgName = (await _newsRepository.GetById(
                                request.NewsFormDto.Id)).ImgName;
                } 

                var updateData = await _newsRepository.GetById(request.NewsFormDto.Id);
                
                _mapper.Map(NewsDto, updateData);

                var data = await _newsRepository.Update(updateData);

                response.Data = _mapper.Map<NewsDto>(data);
                response.Success = true;
                response.Message = "Updated Successfull";
                response.Status = "200";
            }
            return response;
        }
    }
 }

