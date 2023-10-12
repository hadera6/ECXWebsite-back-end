using AutoMapper;
using ECX.Website.Application.CQRS.Faq_.Request.Command;
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

namespace ECX.Website.Application.CQRS.Faq_.Handler.Command
{
    public class DeleteFaqCommandHandler : IRequestHandler<DeleteFaqCommand, BaseCommonResponse>
    {
        
        private IFaqRepository _faqRepository;
        private IMapper _mapper;
        public DeleteFaqCommandHandler(IFaqRepository faqRepository, IMapper mapper)
        {
            _faqRepository = faqRepository;
            _mapper = mapper;
        }
        public async Task<BaseCommonResponse> Handle(DeleteFaqCommand request, CancellationToken cancellationToken)
        {
            var data = await _faqRepository.GetById(request.Id);
            var response = new BaseCommonResponse();

            if (data == null)
            {
                response.Success = false;
                response.Message = new NotFoundException(
                            nameof(Faq), request.Id).Message.ToString();
                response.Status = "404";
            }
            else
            {
                await _faqRepository.Delete(data);

                response.Success = true;
                response.Message = "Successfully Deleted";
                response.Status = "200";

            }
                
            return response;
        }
    }
}
