using AutoMapper;
using ECX.Website.Application.Contracts.Persistence;
using ECX.Website.Application.CQRS.BoardOfDirector_.Request.Command;
using ECX.Website.Application.DTOs.BoardOfDirector;
using ECX.Website.Application.DTOs.BoardOfDirector.Validators;
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

namespace ECX.Website.Application.CQRS.BoardOfDirector_.Handler.Command
{
    public class CreateBoardOfDirectorCommandHandler : IRequestHandler<CreateBoardOfDirectorCommand, BaseCommonResponse>
    {
        private IBoardOfDirectorRepository _boardOfDirectorRepository;
        private IMapper _mapper;
        
        public CreateBoardOfDirectorCommandHandler(IBoardOfDirectorRepository boardOfDirectorRepository, IMapper mapper)
        {
            _boardOfDirectorRepository = boardOfDirectorRepository;
            _mapper = mapper;
        }
        public async Task<BaseCommonResponse> Handle(CreateBoardOfDirectorCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommonResponse();
            var validator = new BoardOfDirectorCreateDtoValidator();
            var validationResult = await validator.ValidateAsync(request.BoardOfDirectorFormDto);

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
                    var imgValidationResult = await imageValidator.ValidateAsync(request.BoardOfDirectorFormDto.ImgFile);

                    if (imgValidationResult.IsValid == false)
                    {
                        response.Success = false;
                        response.Message = "Creation Faild";
                        response.Errors = imgValidationResult.Errors.Select(x => x.ErrorMessage).ToList();
                        response.Status = "400";
                    }
                    else
                    {
                        string contentType = request.BoardOfDirectorFormDto.ImgFile.ContentType.ToString();
                        string ext = contentType.Split('/')[1];
                        string fileName = Guid.NewGuid().ToString() +"."+ext;
                        string path = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\image", fileName);

                        using (Stream stream = new FileStream(path, FileMode.Create))
                        {
                            request.BoardOfDirectorFormDto.ImgFile.CopyTo(stream);
                        }
                        var BoardOfDirectorDto = _mapper.Map<BoardOfDirectorDto>(request.BoardOfDirectorFormDto);
                        BoardOfDirectorDto.ImgName = fileName;

                        string boardOfDirectorId ;
                        bool flag = true;

                        while (true)
                        {
                            boardOfDirectorId = (Guid.NewGuid()).ToString();
                            flag = await _boardOfDirectorRepository.Exists(boardOfDirectorId);
                            if (flag == false)
                            {
                                BoardOfDirectorDto.Id = boardOfDirectorId;
                                break;
                            }
                        }

                        var data =_mapper.Map<BoardOfDirector>(BoardOfDirectorDto);
                        
                        var saveData = await _boardOfDirectorRepository.Add(data);

                        response.Data = _mapper.Map<BoardOfDirectorDto>(saveData);
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
