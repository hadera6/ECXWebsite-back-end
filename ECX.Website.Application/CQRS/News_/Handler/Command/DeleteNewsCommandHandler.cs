using AutoMapper;
using ECX.Website.Application.CQRS.News_.Request.Command;
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

namespace ECX.Website.Application.CQRS.News_.Handler.Command
{
    public class DeleteNewsCommandHandler : IRequestHandler<DeleteNewsCommand, BaseCommonResponse>
    {
        
        private INewsRepository _newsRepository;
        private IMapper _mapper;
        public DeleteNewsCommandHandler(INewsRepository newsRepository, IMapper mapper)
        {
            _newsRepository = newsRepository;
            _mapper = mapper;
        }
        public async Task<BaseCommonResponse> Handle(DeleteNewsCommand request, CancellationToken cancellationToken)
        {
            var data = await _newsRepository.GetById(request.Id);
            var response = new BaseCommonResponse();

            if (data == null)
            {
                response.Success = false;
                response.Message = new NotFoundException(
                            nameof(News), request.Id).Message.ToString();
                response.Status = "404";
            }
            else
            {
                await _newsRepository.Delete(data);

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
