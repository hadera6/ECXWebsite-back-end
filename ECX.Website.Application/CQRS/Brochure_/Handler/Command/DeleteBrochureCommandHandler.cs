using AutoMapper;
using ECX.Website.Application.CQRS.Brochure_.Request.Command;
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

namespace ECX.Website.Application.CQRS.Brochure_.Handler.Command
{
    public class DeleteBrochureCommandHandler : IRequestHandler<DeleteBrochureCommand, BaseCommonResponse>
    {
        
        private IBrochureRepository _brochureRepository;
        private IMapper _mapper;
        public DeleteBrochureCommandHandler(IBrochureRepository brochureRepository, IMapper mapper)
        {
            _brochureRepository = brochureRepository;
            _mapper = mapper;
        }
        public async Task<BaseCommonResponse> Handle(DeleteBrochureCommand request, CancellationToken cancellationToken)
        {
            var data = await _brochureRepository.GetById(request.Id);
            var response = new BaseCommonResponse();

            if (data == null)
            {
                response.Success = false;
                response.Message = new NotFoundException(
                            nameof(Brochure), request.Id).Message.ToString();
                response.Status = "404";
            }
            else
            {
                await _brochureRepository.Delete(data);

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
