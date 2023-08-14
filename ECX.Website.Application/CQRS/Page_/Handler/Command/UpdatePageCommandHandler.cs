using AutoMapper;
using ECX.Website.Application.Contracts.Persistence;
using ECX.Website.Application.CQRS.Page_.Request.Command;
using ECX.Website.Application.DTOs.Page;
using ECX.Website.Application.DTOs.Page.Validators;
using ECX.Website.Application.DTOs.Common.Validators;
using ECX.Website.Application.Exceptions;
using ECX.Website.Application.Response;
using ECX.Website.Domain;
using MediatR;

namespace ECX.Website.Application.CQRS.Page_.Handler.Command
{
    public class UpdatePageCommandHandler : IRequestHandler<UpdatePageCommand, BaseCommonResponse>
    {
        private IPageRepository _pageRepository;
        private IMapper _mapper;
        public UpdatePageCommandHandler(IPageRepository pageRepository, IMapper mapper)
        {
            _pageRepository = pageRepository;
            _mapper = mapper;
        }

        public async Task<BaseCommonResponse> Handle(UpdatePageCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommonResponse();
            var validator = new PageUpdateDtoValidator();
            var validationResult = await validator.ValidateAsync(request.PageFormDto);
            var PageDto = _mapper.Map<PageDto>(request.PageFormDto);
            var flag = await _pageRepository.Exists(request.PageFormDto.Id);

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
                            nameof(Page), request.PageFormDto.Id).Message.ToString();
                response.Status = "404";
            }
            else 
            {
                if (request.PageFormDto.ImgFile != null)
                {
                    try
                    {
                        var imageValidator = new ImageValidator();
                        var imgValidationResult = await imageValidator.ValidateAsync(request.PageFormDto.ImgFile);

                        if (imgValidationResult.IsValid == false)
                        {
                            response.Success = false;
                            response.Message = "Update Failed";
                            response.Errors = imgValidationResult.Errors.Select(x => x.ErrorMessage).ToList();
                            response.Status = "400";
                        }
                        else
                        {
                            var oldImage = (await _pageRepository.GetById(
                                request.PageFormDto.Id)).ImgName;
                            

                            string oldPath = Path.Combine(
                                Directory.GetCurrentDirectory(), @"wwwroot\image",oldImage);
                            File.Delete(oldPath);

                            string contentType = request.PageFormDto.ImgFile.ContentType.ToString();
                            string ext = contentType.Split('/')[1];
                            string fileName = Guid.NewGuid().ToString() + "." + ext;
                            string path = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\image", fileName);

                            using (Stream stream = new FileStream(path, FileMode.Create))
                            {
                                request.PageFormDto.ImgFile.CopyTo(stream);
                            }
                           
                            PageDto.ImgName = fileName;
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
                    PageDto.ImgName = (await _pageRepository.GetById(
                                request.PageFormDto.Id)).ImgName;
                } 

                var updateData = await _pageRepository.GetById(request.PageFormDto.Id);
                
                _mapper.Map(PageDto, updateData);

                var data = await _pageRepository.Update(updateData);

                response.Data = _mapper.Map<PageDto>(data);
                response.Success = true;
                response.Message = "Updated Successfull";
                response.Status = "200";
            }
            return response;
        }
    }
 }

