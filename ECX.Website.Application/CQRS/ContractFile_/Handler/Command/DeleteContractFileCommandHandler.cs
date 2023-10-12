using AutoMapper;
using ECX.Website.Application.CQRS.ContractFile_.Request.Command;
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

namespace ECX.Website.Application.CQRS.ContractFile_.Handler.Command
{
    public class DeleteContractFileCommandHandler : IRequestHandler<DeleteContractFileCommand, BaseCommonResponse>
    {
        
        private IContractFileRepository _contractFileRepository;
        private IMapper _mapper;
        public DeleteContractFileCommandHandler(IContractFileRepository contractFileRepository, IMapper mapper)
        {
            _contractFileRepository = contractFileRepository;
            _mapper = mapper;
        }
        public async Task<BaseCommonResponse> Handle(DeleteContractFileCommand request, CancellationToken cancellationToken)
        {
            var data = await _contractFileRepository.GetById(request.Id);
            var response = new BaseCommonResponse();

            if (data == null)
            {
                response.Success = false;
                response.Message = new NotFoundException(
                            nameof(ContractFile), request.Id).Message.ToString();
                response.Status = "404";
            }
            else
            {
                await _contractFileRepository.Delete(data);

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
