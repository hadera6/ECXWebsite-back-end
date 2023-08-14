using AutoMapper;
using ECX.Website.Application.CQRS.Page_.Request.Command;
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

namespace ECX.Website.Application.CQRS.Page_.Handler.Command
{
    public class DeletePageCommandHandler : IRequestHandler<DeletePageCommand, BaseCommonResponse>
    {
        
        private IPageRepository _pageRepository;
        private IMapper _mapper;
        public DeletePageCommandHandler(IPageRepository pageRepository, IMapper mapper)
        {
            _pageRepository = pageRepository;
            _mapper = mapper;
        }
        public async Task<BaseCommonResponse> Handle(DeletePageCommand request, CancellationToken cancellationToken)
        {
            var data = await _pageRepository.GetById(request.Id);
            var response = new BaseCommonResponse();

            if (data == null)
            {
                response.Success = false;
                response.Message = new NotFoundException(
                            nameof(Page), request.Id).Message.ToString();
                response.Status = "404";
            }
            else
            {
                await _pageRepository.Delete(data);

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
