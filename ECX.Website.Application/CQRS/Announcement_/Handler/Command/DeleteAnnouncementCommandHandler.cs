using AutoMapper;
using ECX.Website.Application.CQRS.Announcement_.Request.Command;
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

namespace ECX.Website.Application.CQRS.Announcement_.Handler.Command
{
    public class DeleteAnnouncementCommandHandler : IRequestHandler<DeleteAnnouncementCommand, BaseCommonResponse>
    {
        
        private IAnnouncementRepository _announcementRepository;
        private IMapper _mapper;
        public DeleteAnnouncementCommandHandler(IAnnouncementRepository announcementRepository, IMapper mapper)
        {
            _announcementRepository = announcementRepository;
            _mapper = mapper;
        }
        public async Task<BaseCommonResponse> Handle(DeleteAnnouncementCommand request, CancellationToken cancellationToken)
        {
            var data = await _announcementRepository.GetById(request.Id);
            var response = new BaseCommonResponse();

            if (data == null)
            {
                response.Success = false;
                response.Message = new NotFoundException(
                            nameof(Announcement), request.Id).Message.ToString();
                response.Status = "404";
            }
            else
            {
                await _announcementRepository.Delete(data);

                string path = Path.Combine(
                    Directory.GetCurrentDirectory(), @"wwwroot\pdf", data.FileName);

                File.Delete(path);

                response.Success = true;
                response.Message = "Successfully Deleted";
                response.Status = "200";

            }
                
            return response;
        }
    }
}
