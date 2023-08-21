using AutoMapper;
using ECX.Website.Application.Contracts.Persistence;
using ECX.Website.Application.CQRS.Account_.Request.Command;
using ECX.Website.Application.DTOs.Account;
using ECX.Website.Application.DTOs.Account.Validators;
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
using ECX.Website.Application.CQRS.Account_.Request.Queries;

namespace ECX.Website.Application.CQRS.Account_.Handler.Queries
{
    public class GetAccountDetailRequestHandler : IRequestHandler<GetAccountDetailRequest, BaseCommonResponse>
    {
        private IAccountRepository _accountRepository;
        private IMapper _mapper;
        
        public GetAccountDetailRequestHandler(IAccountRepository accountRepository, IMapper mapper)
        {
            _accountRepository = accountRepository;
            _mapper = mapper;
        }
        public async Task<BaseCommonResponse> Handle(GetAccountDetailRequest request, CancellationToken cancellationToken)
        {
            var response = new BaseCommonResponse();
            var data = await _accountRepository.GetById(request.Id);
            if (data != null)
            {
                response.Success = true;
                response.Data = _mapper.Map<AccountDto>(data);
                response.Status = "200";
            }
            else
            {
                response.Success = false;
                response.Message = new NotFoundException(
                          nameof(Account), request.Id).Message.ToString();
                response.Status = "404";
            }
            return response;
        }
    }
}
