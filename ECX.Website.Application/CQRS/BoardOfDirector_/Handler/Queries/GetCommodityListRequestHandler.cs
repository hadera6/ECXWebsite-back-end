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
    public class GetBoardOfDirectorListRequestHandler : IRequestHandler<GetBoardOfDirectorListRequest, BaseCommonResponse>
    {
        private IBoardOfDirectorRepository _boardOfDirectorRepository;
        private IMapper _mapper;
        public GetBoardOfDirectorListRequestHandler(IBoardOfDirectorRepository boardOfDirectorRepository, IMapper mapper)
        {
            _boardOfDirectorRepository = boardOfDirectorRepository;
            _mapper = mapper;
        }
        public async Task<BaseCommonResponse> Handle(GetBoardOfDirectorListRequest request, CancellationToken cancellationToken)
        {
            var response = new BaseCommonResponse();
            var data = await _boardOfDirectorRepository.GetAll();

            response.Success = true;
            response.Data = _mapper.Map<List<BoardOfDirectorDto>>(data);
            response.Status = "200";

            return response;
        }
    }
}
