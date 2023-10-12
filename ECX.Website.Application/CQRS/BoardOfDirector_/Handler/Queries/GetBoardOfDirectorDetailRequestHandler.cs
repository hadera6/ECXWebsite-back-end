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
using ECX.Website.Application.CQRS.BoardOfDirector_.Request.Queries;

namespace ECX.Website.Application.CQRS.BoardOfDirector_.Handler.Queries
{
    public class GetBoardOfDirectorDetailRequestHandler : IRequestHandler<GetBoardOfDirectorDetailRequest, BaseCommonResponse>
    {
        private IBoardOfDirectorRepository _boardOfDirectorRepository;
        private IMapper _mapper;
        
        public GetBoardOfDirectorDetailRequestHandler(IBoardOfDirectorRepository boardOfDirectorRepository, IMapper mapper)
        {
            _boardOfDirectorRepository = boardOfDirectorRepository;
            _mapper = mapper;
        }
        public async Task<BaseCommonResponse> Handle(GetBoardOfDirectorDetailRequest request, CancellationToken cancellationToken)
        {
            var response = new BaseCommonResponse();
            var data = await _boardOfDirectorRepository.GetById(request.Id);
            if (data != null)
            {
                response.Success = true;
                response.Data = _mapper.Map<BoardOfDirectorDto>(data);
                response.Status = "200";
            }
            else
            {
                response.Success = false;
                response.Message = new NotFoundException(
                          nameof(BoardOfDirector), request.Id).Message.ToString();
                response.Status = "404";
            }
            return response;
        }
    }
}
