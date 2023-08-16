using AutoMapper;
using ECX.Website.Application.Contracts.Persistence;
using ECX.Website.Application.CQRS.PageCatagory_.Request.Command;
using ECX.Website.Application.DTOs.PageCatagory;
using ECX.Website.Application.DTOs.PageCatagory.Validators;
using ECX.Website.Application.DTOs.Common.Validators;
using ECX.Website.Application.Exceptions;
using ECX.Website.Application.Response;
using ECX.Website.Domain;
using MediatR;

namespace ECX.Website.Application.CQRS.PageCatagory_.Handler.Command
{
    public class UpdatePageCatagoryCommandHandler : IRequestHandler<UpdatePageCatagoryCommand, BaseCommonResponse>
    {
        private IPageCatagoryRepository _pageCatagoryRepository;
        private IMapper _mapper;
        public UpdatePageCatagoryCommandHandler(IPageCatagoryRepository pageCatagoryRepository, IMapper mapper)
        {
            _pageCatagoryRepository = pageCatagoryRepository;
            _mapper = mapper;
        }

        public async Task<BaseCommonResponse> Handle(UpdatePageCatagoryCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommonResponse();
            var validator = new PageCatagoryUpdateDtoValidator();
            var validationResult = await validator.ValidateAsync(request.PageCatagoryFormDto);
            var PageCatagoryDto = _mapper.Map<PageCatagoryDto>(request.PageCatagoryFormDto);
            var flag = await _pageCatagoryRepository.Exists(request.PageCatagoryFormDto.Id);

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
                    nameof(PageCatagory), request.PageCatagoryFormDto.Id).Message.ToString();
                response.Status = "404";
            }
            else 
            {
                if (request.PageCatagoryFormDto.ImgFile != null)
                {
                    try
                    {
                        var imageValidator = new ImageValidator();
                        var imgValidationResult = await imageValidator.ValidateAsync(request.PageCatagoryFormDto.ImgFile);

                        if (imgValidationResult.IsValid == false)
                        {
                            response.Success = false;
                            response.Message = "Update Failed";
                            response.Errors = imgValidationResult.Errors.Select(x => x.ErrorMessage).ToList();
                            response.Status = "400";
                        }
                        else
                        {
                            var oldImage = (await _pageCatagoryRepository.GetById(
                                request.PageCatagoryFormDto.Id)).ImgName;
                            

                            string oldPath = Path.Combine(
                                Directory.GetCurrentDirectory(), @"wwwroot\image",oldImage);
                            File.Delete(oldPath);

                            string contentType = request.PageCatagoryFormDto.ImgFile.ContentType.ToString();
                            string ext = contentType.Split('/')[1];
                            string fileName = Guid.NewGuid().ToString() + "." + ext;
                            string path = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\image", fileName);

                            using (Stream stream = new FileStream(path, FileMode.Create))
                            {
                                request.PageCatagoryFormDto.ImgFile.CopyTo(stream);
                            }
                           
                            PageCatagoryDto.ImgName = fileName;
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
                    PageCatagoryDto.ImgName = (await _pageCatagoryRepository.GetById(
                                request.PageCatagoryFormDto.Id)).ImgName;
                } 

                var updateData = await _pageCatagoryRepository.GetById(request.PageCatagoryFormDto.Id);
                
                _mapper.Map(PageCatagoryDto, updateData);

                var data = await _pageCatagoryRepository.Update(updateData);

                response.Data = _mapper.Map<PageCatagoryDto>(data);
                response.Success = true;
                response.Message = "Updated Successfull";
                response.Status = "200";
            }
            return response;
        }
    }
 }

