using AutoMapper;
using ECX.Website.Application.CQRS.PageCatagory_.Request.Command;
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

namespace ECX.Website.Application.CQRS.PageCatagory_.Handler.Command
{
    public class DeletePageCatagoryCommandHandler : IRequestHandler<DeletePageCatagoryCommand, BaseCommonResponse>
    {
        
        private IPageCatagoryRepository _pageCatagoryRepository;
        private IMapper _mapper;
        public DeletePageCatagoryCommandHandler(IPageCatagoryRepository pageCatagoryRepository, IMapper mapper)
        {
            _pageCatagoryRepository = pageCatagoryRepository;
            _mapper = mapper;
        }
        public async Task<BaseCommonResponse> Handle(DeletePageCatagoryCommand request, CancellationToken cancellationToken)
        {
            var data = await _pageCatagoryRepository.GetById(request.Id);
            var response = new BaseCommonResponse();

            if (data == null)
            {
                response.Success = false;
                response.Message = new NotFoundException(
                            nameof(PageCatagory), request.Id).Message.ToString();
                response.Status = "404";
            }
            else
            {
                await _pageCatagoryRepository.Delete(data);

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
