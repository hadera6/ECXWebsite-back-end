﻿using AutoMapper;
using ECX.Website.Application.Contracts.Persistence;
using ECX.Website.Application.CQRS.Announcement_.Request.Command;
using ECX.Website.Application.DTOs.Announcement;
using ECX.Website.Application.DTOs.Announcement.Validators;
using ECX.Website.Application.DTOs.Common.Validators;
using ECX.Website.Application.Exceptions;
using ECX.Website.Application.Response;
using ECX.Website.Domain;
using MediatR;

namespace ECX.Website.Application.CQRS.Announcement_.Handler.Command
{
    public class UpdateAnnouncementCommandHandler : IRequestHandler<UpdateAnnouncementCommand, BaseCommonResponse>
    {
        private IAnnouncementRepository _announcementRepository;
        private IMapper _mapper;
        public UpdateAnnouncementCommandHandler(IAnnouncementRepository announcementRepository, IMapper mapper)
        {
            _announcementRepository = announcementRepository;
            _mapper = mapper;
        }

        public async Task<BaseCommonResponse> Handle(UpdateAnnouncementCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommonResponse();
            var validator = new AnnouncementUpdateDtoValidator();
            var validationResult = await validator.ValidateAsync(request.AnnouncementFormDto);
            var AnnouncementDto = _mapper.Map<AnnouncementDto>(request.AnnouncementFormDto);
            var flag = await _announcementRepository.Exists(request.AnnouncementFormDto.Id);

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
                            nameof(Announcement), request.AnnouncementFormDto.Id).Message.ToString();
                response.Status = "404";
            }
            else 
            {
                if (request.AnnouncementFormDto.ImgFile != null)
                {
                    try
                    {
                        var imageValidator = new ImageValidator();
                        var imgValidationResult = await imageValidator.ValidateAsync(request.AnnouncementFormDto.ImgFile);

                        if (imgValidationResult.IsValid == false)
                        {
                            response.Success = false;
                            response.Message = "Update Failed";
                            response.Errors = imgValidationResult.Errors.Select(x => x.ErrorMessage).ToList();
                            response.Status = "400";
                        }
                        else
                        {
                            var oldImage = (await _announcementRepository.GetById(
                                request.AnnouncementFormDto.Id)).ImgName;
                            

                            string oldPath = Path.Combine(
                                Directory.GetCurrentDirectory(), @"wwwroot\image",oldImage);
                            File.Delete(oldPath);

                            string contentType = request.AnnouncementFormDto.ImgFile.ContentType.ToString();
                            string ext = contentType.Split('/')[1];
                            string fileName = Guid.NewGuid().ToString() + "." + ext;
                            string path = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\image", fileName);

                            using (Stream stream = new FileStream(path, FileMode.Create))
                            {
                                request.AnnouncementFormDto.ImgFile.CopyTo(stream);
                            }
                           
                            AnnouncementDto.ImgName = fileName;
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
                    AnnouncementDto.ImgName = (await _announcementRepository.GetById(
                                request.AnnouncementFormDto.Id)).ImgName;
                } 

                var updateData = await _announcementRepository.GetById(request.AnnouncementFormDto.Id);
                
                _mapper.Map(AnnouncementDto, updateData);

                var data = await _announcementRepository.Update(updateData);

                response.Data = _mapper.Map<AnnouncementDto>(data);
                response.Success = true;
                response.Message = "Updated Successfull";
                response.Status = "200";
            }
            return response;
        }
    }
 }

