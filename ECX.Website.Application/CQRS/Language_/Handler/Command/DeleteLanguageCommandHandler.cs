using AutoMapper;
using ECX.Website.Application.CQRS.Language_.Request.Command;
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

namespace ECX.Website.Application.CQRS.Language_.Handler.Command
{
    public class DeleteLanguageCommandHandler : IRequestHandler<DeleteLanguageCommand, BaseCommonResponse>
    {
        
        private ILanguageRepository _languageRepository;
        private IMapper _mapper;
        public DeleteLanguageCommandHandler(ILanguageRepository languageRepository, IMapper mapper)
        {
            _languageRepository = languageRepository;
            _mapper = mapper;
        }
        public async Task<BaseCommonResponse> Handle(DeleteLanguageCommand request, CancellationToken cancellationToken)
        {
            var data = await _languageRepository.GetById(request.Id);
            var response = new BaseCommonResponse();

            if (data == null)
            {
                response.Success = false;
                response.Message = new NotFoundException(
                            nameof(Language), request.Id).Message.ToString();
                response.Status = "404";
            }
            else
            {
                await _languageRepository.Delete(data);
                response.Success = true;
                response.Message = "Successfully Deleted";
                response.Status = "200";

            }
                
            return response;
        }
    }
}
