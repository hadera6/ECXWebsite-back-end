using AutoMapper;
using ECX.Website.Application.CQRS.ExternalLink_.Request.Command;
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

namespace ECX.Website.Application.CQRS.ExternalLink_.Handler.Command
{
    public class DeleteExternalLinkCommandHandler : IRequestHandler<DeleteExternalLinkCommand, BaseCommonResponse>
    {
        
        private IExternalLinkRepository _externalLinkRepository;
        private IMapper _mapper;
        public DeleteExternalLinkCommandHandler(IExternalLinkRepository externalLinkRepository, IMapper mapper)
        {
            _externalLinkRepository = externalLinkRepository;
            _mapper = mapper;
        }
        public async Task<BaseCommonResponse> Handle(DeleteExternalLinkCommand request, CancellationToken cancellationToken)
        {
            var data = await _externalLinkRepository.GetById(request.Id);
            var response = new BaseCommonResponse();

            if (data == null)
            {
                response.Success = false;
                response.Message = new NotFoundException(
                            nameof(ExternalLink), request.Id).Message.ToString();
                response.Status = "404";
            }
            else
            {
                await _externalLinkRepository.Delete(data);

                response.Success = true;
                response.Message = "Successfully Deleted";
                response.Status = "200";

            }
                
            return response;
        }
    }
}
