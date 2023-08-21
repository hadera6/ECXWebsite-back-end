using AutoMapper;
using ECX.Website.Application.Contracts.Persistence;
using ECX.Website.Application.CQRS.Blog_.Request.Command;
using ECX.Website.Application.DTOs.Blog;
using ECX.Website.Application.DTOs.Blog.Validators;
using ECX.Website.Application.DTOs.Common.Validators;
using ECX.Website.Application.Exceptions;
using ECX.Website.Application.Response;
using ECX.Website.Domain;
using MediatR;

namespace ECX.Website.Application.CQRS.Blog_.Handler.Command
{
    public class UpdateBlogCommandHandler : IRequestHandler<UpdateBlogCommand, BaseCommonResponse>
    {
        private IBlogRepository _blogRepository;
        private IMapper _mapper;
        public UpdateBlogCommandHandler(IBlogRepository blogRepository, IMapper mapper)
        {
            _blogRepository = blogRepository;
            _mapper = mapper;
        }

        public async Task<BaseCommonResponse> Handle(UpdateBlogCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommonResponse();
            var validator = new BlogUpdateDtoValidator();
            var validationResult = await validator.ValidateAsync(request.BlogFormDto);
            var BlogDto = _mapper.Map<BlogDto>(request.BlogFormDto);
            var flag = await _blogRepository.Exists(request.BlogFormDto.Id);

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
                            nameof(Blog), request.BlogFormDto.Id).Message.ToString();
                response.Status = "404";
            }
            else 
            {
                if (request.BlogFormDto.ImgFile != null)
                {
                    try
                    {
                        var imageValidator = new ImageValidator();
                        var imgValidationResult = await imageValidator.ValidateAsync(request.BlogFormDto.ImgFile);

                        if (imgValidationResult.IsValid == false)
                        {
                            response.Success = false;
                            response.Message = "Update Failed";
                            response.Errors = imgValidationResult.Errors.Select(x => x.ErrorMessage).ToList();
                            response.Status = "400";
                        }
                        else
                        {
                            var oldImage = (await _blogRepository.GetById(
                                request.BlogFormDto.Id)).ImgName;
                            

                            string oldPath = Path.Combine(
                                Directory.GetCurrentDirectory(), @"wwwroot\image",oldImage);
                            File.Delete(oldPath);

                            string contentType = request.BlogFormDto.ImgFile.ContentType.ToString();
                            string ext = contentType.Split('/')[1];
                            string fileName = Guid.NewGuid().ToString() + "." + ext;
                            string path = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\image", fileName);

                            using (Stream stream = new FileStream(path, FileMode.Create))
                            {
                                request.BlogFormDto.ImgFile.CopyTo(stream);
                            }
                           
                            BlogDto.ImgName = fileName;
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
                    BlogDto.ImgName = (await _blogRepository.GetById(
                                request.BlogFormDto.Id)).ImgName;
                } 

                var updateData = await _blogRepository.GetById(request.BlogFormDto.Id);
                
                _mapper.Map(BlogDto, updateData);

                var data = await _blogRepository.Update(updateData);

                response.Data = _mapper.Map<BlogDto>(data);
                response.Success = true;
                response.Message = "Updated Successfull";
                response.Status = "200";
            }
            return response;
        }
    }
 }

