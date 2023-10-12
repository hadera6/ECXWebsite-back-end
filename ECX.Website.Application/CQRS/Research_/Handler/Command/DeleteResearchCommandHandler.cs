using AutoMapper;
using ECX.Website.Application.CQRS.Research_.Request.Command;
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

namespace ECX.Website.Application.CQRS.Research_.Handler.Command
{
    public class DeleteResearchCommandHandler : IRequestHandler<DeleteResearchCommand, BaseCommonResponse>
    {
        
        private IResearchRepository _researchRepository;
        private IMapper _mapper;
        public DeleteResearchCommandHandler(IResearchRepository researchRepository, IMapper mapper)
        {
            _researchRepository = researchRepository;
            _mapper = mapper;
        }
        public async Task<BaseCommonResponse> Handle(DeleteResearchCommand request, CancellationToken cancellationToken)
        {
            var data = await _researchRepository.GetById(request.Id);
            var response = new BaseCommonResponse();

            if (data == null)
            {
                response.Success = false;
                response.Message = new NotFoundException(
                            nameof(Research), request.Id).Message.ToString();
                response.Status = "404";
            }
            else
            {
                await _researchRepository.Delete(data);

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
