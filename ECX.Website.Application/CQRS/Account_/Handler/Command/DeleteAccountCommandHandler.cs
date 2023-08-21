using AutoMapper;
using ECX.Website.Application.CQRS.Account_.Request.Command;
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

namespace ECX.Website.Application.CQRS.Account_.Handler.Command
{
    public class DeleteAccountCommandHandler : IRequestHandler<DeleteAccountCommand, BaseCommonResponse>
    {
        
        private IAccountRepository _accountRepository;
        private IMapper _mapper;
        public DeleteAccountCommandHandler(IAccountRepository accountRepository, IMapper mapper)
        {
            _accountRepository = accountRepository;
            _mapper = mapper;
        }
        public async Task<BaseCommonResponse> Handle(DeleteAccountCommand request, CancellationToken cancellationToken)
        {
            var data = await _accountRepository.GetById(request.Id);
            var response = new BaseCommonResponse();

            if (data == null)
            {
                response.Success = false;
                response.Message = new NotFoundException(
                            nameof(Account), request.Id).Message.ToString();
                response.Status = "404";
            }
            else
            {
                await _accountRepository.Delete(data);

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
