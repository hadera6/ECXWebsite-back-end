using AutoMapper;
using ECX.Website.Application.CQRS.FeedBack_.Request.Command;
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

namespace ECX.Website.Application.CQRS.FeedBack_.Handler.Command
{
    public class DeleteFeedBackCommandHandler : IRequestHandler<DeleteFeedBackCommand, BaseCommonResponse>
    {
        
        private IFeedBackRepository _feedBackRepository;
        private IMapper _mapper;
        public DeleteFeedBackCommandHandler(IFeedBackRepository feedBackRepository, IMapper mapper)
        {
            _feedBackRepository = feedBackRepository;
            _mapper = mapper;
        }
        public async Task<BaseCommonResponse> Handle(DeleteFeedBackCommand request, CancellationToken cancellationToken)
        {
            var data = await _feedBackRepository.GetById(request.Id);
            var response = new BaseCommonResponse();

            if (data == null)
            {
                response.Success = false;
                response.Message = new NotFoundException(
                            nameof(FeedBack), request.Id).Message.ToString();
                response.Status = "404";
            }
            else
            {
                await _feedBackRepository.Delete(data);

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
