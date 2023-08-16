using AutoMapper;
using ECX.Website.Application.Contracts.Persistence;
using ECX.Website.Application.CQRS.BoardOfDirector_.Request.Command;
using ECX.Website.Application.DTOs.BoardOfDirector;
using ECX.Website.Application.DTOs.BoardOfDirector.Validators;
using ECX.Website.Application.DTOs.Common.Validators;
using ECX.Website.Application.Exceptions;
using ECX.Website.Application.Response;
using ECX.Website.Domain;
using MediatR;

namespace ECX.Website.Application.CQRS.BoardOfDirector_.Handler.Command
{
    public class UpdateBoardOfDirectorCommandHandler : IRequestHandler<UpdateBoardOfDirectorCommand, BaseCommonResponse>
    {
        private IBoardOfDirectorRepository _boardOfDirectorRepository;
        private IMapper _mapper;
        public UpdateBoardOfDirectorCommandHandler(IBoardOfDirectorRepository boardOfDirectorRepository, IMapper mapper)
        {
            _boardOfDirectorRepository = boardOfDirectorRepository;
            _mapper = mapper;
        }

        public async Task<BaseCommonResponse> Handle(UpdateBoardOfDirectorCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommonResponse();
            var validator = new BoardOfDirectorUpdateDtoValidator();
            var validationResult = await validator.ValidateAsync(request.BoardOfDirectorFormDto);
            var BoardOfDirectorDto = _mapper.Map<BoardOfDirectorDto>(request.BoardOfDirectorFormDto);
            var flag = await _boardOfDirectorRepository.Exists(request.BoardOfDirectorFormDto.Id);

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
                            nameof(BoardOfDirector), request.BoardOfDirectorFormDto.Id).Message.ToString();
                response.Status = "404";
            }
            else 
            {
                if (request.BoardOfDirectorFormDto.ImgFile != null)
                {
                    try
                    {
                        var imageValidator = new ImageValidator();
                        var imgValidationResult = await imageValidator.ValidateAsync(request.BoardOfDirectorFormDto.ImgFile);

                        if (imgValidationResult.IsValid == false)
                        {
                            response.Success = false;
                            response.Message = "Update Failed";
                            response.Errors = imgValidationResult.Errors.Select(x => x.ErrorMessage).ToList();
                            response.Status = "400";
                        }
                        else
                        {
                            var oldImage = (await _boardOfDirectorRepository.GetById(
                                request.BoardOfDirectorFormDto.Id)).ImgName;
                            

                            string oldPath = Path.Combine(
                                Directory.GetCurrentDirectory(), @"wwwroot\image",oldImage);
                            File.Delete(oldPath);

                            string contentType = request.BoardOfDirectorFormDto.ImgFile.ContentType.ToString();
                            string ext = contentType.Split('/')[1];
                            string fileName = Guid.NewGuid().ToString() + "." + ext;
                            string path = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\image", fileName);

                            using (Stream stream = new FileStream(path, FileMode.Create))
                            {
                                request.BoardOfDirectorFormDto.ImgFile.CopyTo(stream);
                            }
                           
                            BoardOfDirectorDto.ImgName = fileName;
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
                    BoardOfDirectorDto.ImgName = (await _boardOfDirectorRepository.GetById(
                                request.BoardOfDirectorFormDto.Id)).ImgName;
                } 

                var updateData = await _boardOfDirectorRepository.GetById(request.BoardOfDirectorFormDto.Id);
                
                _mapper.Map(BoardOfDirectorDto, updateData);

                var data = await _boardOfDirectorRepository.Update(updateData);

                response.Data = _mapper.Map<BoardOfDirectorDto>(data);
                response.Success = true;
                response.Message = "Updated Successfull";
                response.Status = "200";
            }
            return response;
        }
    }
 }

