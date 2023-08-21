using AutoMapper;
using ECX.Website.Application.Contracts.Persistence;
using ECX.Website.Application.CQRS.Announcement_.Request.Command;
using ECX.Website.Application.DTOs.Announcement;
using ECX.Website.Application.DTOs.Announcement.Validators;
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
using ECX.Website.Application.CQRS.Announcement_.Request.Queries;

namespace ECX.Website.Application.CQRS.Announcement_.Handler.Queries
{
    public class GetAnnouncementDetailRequestHandler : IRequestHandler<GetAnnouncementDetailRequest, BaseCommonResponse>
    {
        private IAnnouncementRepository _announcementRepository;
        private IMapper _mapper;
        
        public GetAnnouncementDetailRequestHandler(IAnnouncementRepository announcementRepository, IMapper mapper)
        {
            _announcementRepository = announcementRepository;
            _mapper = mapper;
        }
        public async Task<BaseCommonResponse> Handle(GetAnnouncementDetailRequest request, CancellationToken cancellationToken)
        {
            var response = new BaseCommonResponse();
            var data = await _announcementRepository.GetById(request.Id);
            if (data != null)
            {
                response.Success = true;
                response.Data = _mapper.Map<AnnouncementDto>(data);
                response.Status = "200";
            }
            else
            {
                response.Success = false;
                response.Message = new NotFoundException(
                          nameof(Announcement), request.Id).Message.ToString();
                response.Status = "404";
            }
            return response;
        }
    }
}
