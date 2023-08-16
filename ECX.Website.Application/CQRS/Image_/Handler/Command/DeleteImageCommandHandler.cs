using AutoMapper;
using ECX.Website.Application.CQRS.Image_.Request.Command;
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

namespace ECX.Website.Application.CQRS.Image_.Handler.Command
{
    public class DeleteImageCommandHandler : IRequestHandler<DeleteImageCommand, BaseCommonResponse>
    {
        
        private IImageRepository _imageRepository;
        private IMapper _mapper;
        public DeleteImageCommandHandler(IImageRepository imageRepository, IMapper mapper)
        {
            _imageRepository = imageRepository;
            _mapper = mapper;
        }
        public async Task<BaseCommonResponse> Handle(DeleteImageCommand request, CancellationToken cancellationToken)
        {
            var data = await _imageRepository.GetById(request.Id);
            var response = new BaseCommonResponse();

            if (data == null)
            {
                response.Success = false;
                response.Message = new NotFoundException(
                            nameof(Image), request.Id).Message.ToString();
                response.Status = "404";
            }
            else
            {
                await _imageRepository.Delete(data);

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
