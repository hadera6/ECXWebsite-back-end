using AutoMapper;
using ECX.Website.Application.Contracts.Persistence;
using ECX.Website.Application.CQRS.Publication_.Request.Command;
using ECX.Website.Application.DTOs.Publication;
using ECX.Website.Application.DTOs.Publication.Validators;
using ECX.Website.Application.Exceptions;

using ECX.Website.Application.Response;
using ECX.Website.Domain;
using MediatR;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using ECX.Website.Application.DTOs.Common.Validators;

namespace ECX.Website.Application.CQRS.Publication_.Handler.Command
{
    public class CreatePublicationCommandHandler : IRequestHandler<CreatePublicationCommand, BaseCommonResponse>
    {
        private IPublicationRepository _publicationRepository;
        private IMapper _mapper;

        public CreatePublicationCommandHandler(IPublicationRepository publicationRepository, IMapper mapper)
        {
            _publicationRepository = publicationRepository;
            _mapper = mapper;
        }
        public async Task<BaseCommonResponse> Handle(CreatePublicationCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommonResponse();
            var validator = new PublicationCreateDtoValidator();
            var validationResult = await validator.ValidateAsync(request.PublicationFormDto);

            if (validationResult.IsValid == false)
            {
                response.Success = false;
                response.Message = "Creation Faild";
                response.Errors = validationResult.Errors.Select(x => x.ErrorMessage).ToList();
                response.Status = "400";
            }
            else
            {
                try
                {
                    var imageValidator = new ImageValidator();
                    var imgValidationResult = await imageValidator.ValidateAsync(request.PublicationFormDto.ImgFile);

                    if (imgValidationResult.IsValid == false)
                    {
                        response.Success = false;
                        response.Message = "Creation Faild";
                        response.Errors = imgValidationResult.Errors.Select(x => x.ErrorMessage).ToList();
                        response.Status = "400";
                    }
                    else
                    {
                        string contentType = request.PublicationFormDto.ImgFile.ContentType.ToString();
                        string ext = contentType.Split('/')[1];
                        string fileName = Guid.NewGuid().ToString() + "." + ext;
                        string path = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\image", fileName);

                        using (Stream stream = new FileStream(path, FileMode.Create))
                        {
                            request.PublicationFormDto.ImgFile.CopyTo(stream);
                        }
                        var PublicationDto = _mapper.Map<PublicationDto>(request.PublicationFormDto);
                        PublicationDto.ImgName = fileName;

                        string publicationId;
                        bool flag = true;

                        while (true)
                        {
                            publicationId = Guid.NewGuid().ToString();
                            flag = await _publicationRepository.Exists(publicationId);
                            if (flag == false)
                            {
                                PublicationDto.Id = publicationId;
                                break;
                            }
                        }

                        var data = _mapper.Map<Publication>(PublicationDto);

                        var saveData = await _publicationRepository.Add(data);

                        response.Data = _mapper.Map<PublicationDto>(saveData);
                        response.Success = true;
                        response.Message = "Created Successfully";
                        response.Status = "200";
                    }
                }
                catch (Exception ex)
                {
                    response.Success = false;
                    response.Message = "Creation Failed";
                    response.Errors = new List<string> { ex.Message };
                    response.Status = "400";
                }
            }
            return response;
        }
    }
}
