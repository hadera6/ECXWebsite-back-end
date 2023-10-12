using AutoMapper;
using ECX.Website.Application.CQRS.Publication_.Request.Command;
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

namespace ECX.Website.Application.CQRS.Publication_.Handler.Command
{
    public class DeletePublicationCommandHandler : IRequestHandler<DeletePublicationCommand, BaseCommonResponse>
    {
        
        private IPublicationRepository _publicationRepository;
        private IMapper _mapper;
        public DeletePublicationCommandHandler(IPublicationRepository publicationRepository, IMapper mapper)
        {
            _publicationRepository = publicationRepository;
            _mapper = mapper;
        }
        public async Task<BaseCommonResponse> Handle(DeletePublicationCommand request, CancellationToken cancellationToken)
        {
            var data = await _publicationRepository.GetById(request.Id);
            var response = new BaseCommonResponse();

            if (data == null)
            {
                response.Success = false;
                response.Message = new NotFoundException(
                            nameof(Publication), request.Id).Message.ToString();
                response.Status = "404";
            }
            else
            {
                await _publicationRepository.Delete(data);

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
