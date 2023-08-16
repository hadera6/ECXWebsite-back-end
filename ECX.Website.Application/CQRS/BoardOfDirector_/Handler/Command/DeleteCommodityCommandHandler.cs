using AutoMapper;
using ECX.Website.Application.CQRS.BoardOfDirector_.Request.Command;
using ECX.Website.Application.Exceptions;
using ECX.Website.Application.Contracts.Persistence;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ECX.Website.Domain;
using ECX.Website.Application.Response;

namespace ECX.Website.Application.CQRS.BoardOfDirector_.Handler.Command
{
    public class DeleteBoardOfDirectorCommandHandler : IRequestHandler<DeleteBoardOfDirectorCommand, BaseCommonResponse>
    {
        
        private IBoardOfDirectorRepository _boardOfDirectorRepository;
        private IMapper _mapper;
        public DeleteBoardOfDirectorCommandHandler(IBoardOfDirectorRepository boardOfDirectorRepository, IMapper mapper)
        {
            _boardOfDirectorRepository = boardOfDirectorRepository;
            _mapper = mapper;
        }
        public async Task<BaseCommonResponse> Handle(DeleteBoardOfDirectorCommand request, CancellationToken cancellationToken)
        {
            var data = await _boardOfDirectorRepository.GetById(request.Id);
            var response = new BaseCommonResponse();

            if (data == null)
            {
                response.Success = false;
                response.Message = new NotFoundException(
                            nameof(BoardOfDirector), request.Id).Message.ToString();
                response.Status = "404";
            }
            else
            {
                await _boardOfDirectorRepository.Delete(data);

                string path = Path.Combine(
                    Directory.GetCurrentDirectory(), @"wwwroot\image", data.ImgName);

                File.Delete(path);

                response.Success = true;
                response.Message = "Successfully Deleted";
                response.Status = "200";

            }
                
            return response;
        }
    }
}
