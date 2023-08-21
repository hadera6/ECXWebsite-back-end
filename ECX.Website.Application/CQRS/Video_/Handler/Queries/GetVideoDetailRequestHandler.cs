using AutoMapper;
using ECX.Website.Application.Contracts.Persistence;
using ECX.Website.Application.CQRS.Video_.Request.Command;
using ECX.Website.Application.DTOs.Video;
using ECX.Website.Application.DTOs.Video.Validators;
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
using ECX.Website.Application.CQRS.Video_.Request.Queries;

namespace ECX.Website.Application.CQRS.Video_.Handler.Queries
{
    public class GetVideoDetailRequestHandler : IRequestHandler<GetVideoDetailRequest, BaseCommonResponse>
    {
        private IVideoRepository _videoRepository;
        private IMapper _mapper;
        
        public GetVideoDetailRequestHandler(IVideoRepository videoRepository, IMapper mapper)
        {
            _videoRepository = videoRepository;
            _mapper = mapper;
        }
        public async Task<BaseCommonResponse> Handle(GetVideoDetailRequest request, CancellationToken cancellationToken)
        {
            var response = new BaseCommonResponse();
            var data = await _videoRepository.GetById(request.Id);
            if (data != null)
            {
                response.Success = true;
                response.Data = _mapper.Map<VideoDto>(data);
                response.Status = "200";
            }
            else
            {
                response.Success = false;
                response.Message = new NotFoundException(
                          nameof(Video), request.Id).Message.ToString();
                response.Status = "404";
            }
            return response;
        }
    }
}
