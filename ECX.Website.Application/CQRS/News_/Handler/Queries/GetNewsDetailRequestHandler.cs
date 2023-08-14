using AutoMapper;
using ECX.Website.Application.Contracts.Persistence;
using ECX.Website.Application.CQRS.News_.Request.Command;
using ECX.Website.Application.DTOs.News;
using ECX.Website.Application.DTOs.News.Validators;
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
using ECX.Website.Application.CQRS.News_.Request.Queries;

namespace ECX.Website.Application.CQRS.News_.Handler.Queries
{
    public class GetNewsDetailRequestHandler : IRequestHandler<GetNewsDetailRequest, BaseCommonResponse>
    {
        private INewsRepository _newsRepository;
        private IMapper _mapper;
        
        public GetNewsDetailRequestHandler(INewsRepository newsRepository, IMapper mapper)
        {
            _newsRepository = newsRepository;
            _mapper = mapper;
        }
        public async Task<BaseCommonResponse> Handle(GetNewsDetailRequest request, CancellationToken cancellationToken)
        {
            var response = new BaseCommonResponse();
            var data = await _newsRepository.GetById(request.Id);
            if (data != null)
            {
                response.Success = true;
                response.Data = _mapper.Map<NewsDto>(data);
                response.Status = "200";
            }
            else
            {
                response.Success = false;
                response.Message = new NotFoundException(
                    nameof(News), request.Id).Message.ToString();
                response.Status = "404";
            }
            return response;
        }
    }
}
