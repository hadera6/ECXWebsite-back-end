using AutoMapper;
using ECX.Website.Application.CQRS.Video_.Request.Command;
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

namespace ECX.Website.Application.CQRS.Video_.Handler.Command
{
    public class DeleteVideoCommandHandler : IRequestHandler<DeleteVideoCommand, BaseCommonResponse>
    {
        
        private IVideoRepository _videoRepository;
        private IMapper _mapper;
        public DeleteVideoCommandHandler(IVideoRepository videoRepository, IMapper mapper)
        {
            _videoRepository = videoRepository;
            _mapper = mapper;
        }
        public async Task<BaseCommonResponse> Handle(DeleteVideoCommand request, CancellationToken cancellationToken)
        {
            var data = await _videoRepository.GetById(request.Id);
            var response = new BaseCommonResponse();

            if (data == null)
            {
                response.Success = false;
                response.Message = new NotFoundException(
                            nameof(Video), request.Id).Message.ToString();
                response.Status = "404";
            }
            else
            {
                await _videoRepository.Delete(data);

                string path = Path.Combine(
                    Directory.GetCurrentDirectory(), @"wwwroot\video", data.VideoName);

                File.Delete(path);

                response.Success = true;
                response.Message = "Successfully Deleted";
                response.Status = "200";

            }
                
            return response;
        }
    }
}
