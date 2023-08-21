﻿using AutoMapper;
using ECX.Website.Application.CQRS.TradingCenter_.Request.Command;
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

namespace ECX.Website.Application.CQRS.TradingCenter_.Handler.Command
{
    public class DeleteTradingCenterCommandHandler : IRequestHandler<DeleteTradingCenterCommand, BaseCommonResponse>
    {
        
        private ITradingCenterRepository _tradingCenterRepository;
        private IMapper _mapper;
        public DeleteTradingCenterCommandHandler(ITradingCenterRepository tradingCenterRepository, IMapper mapper)
        {
            _tradingCenterRepository = tradingCenterRepository;
            _mapper = mapper;
        }
        public async Task<BaseCommonResponse> Handle(DeleteTradingCenterCommand request, CancellationToken cancellationToken)
        {
            var data = await _tradingCenterRepository.GetById(request.Id);
            var response = new BaseCommonResponse();

            if (data == null)
            {
                response.Success = false;
                response.Message = new NotFoundException(
                            nameof(TradingCenter), request.Id).Message.ToString();
                response.Status = "404";
            }
            else
            {
                await _tradingCenterRepository.Delete(data);

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
