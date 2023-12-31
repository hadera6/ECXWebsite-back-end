﻿using AutoMapper;
using ECX.Website.Application.CQRS.Commodity_.Request.Command;
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

namespace ECX.Website.Application.CQRS.Commodity_.Handler.Command
{
    public class DeleteCommodityCommandHandler : IRequestHandler<DeleteCommodityCommand, BaseCommonResponse>
    {
        
        private ICommodityRepository _commodityRepository;
        private IMapper _mapper;
        public DeleteCommodityCommandHandler(ICommodityRepository commodityRepository, IMapper mapper)
        {
            _commodityRepository = commodityRepository;
            _mapper = mapper;
        }
        public async Task<BaseCommonResponse> Handle(DeleteCommodityCommand request, CancellationToken cancellationToken)
        {
            var data = await _commodityRepository.GetById(request.Id);
            var response = new BaseCommonResponse();

            if (data == null)
            {
                response.Success = false;
                response.Message = new NotFoundException(
                            nameof(Commodity), request.Id).Message.ToString();
                response.Status = "404";
            }
            else
            {
                await _commodityRepository.Delete(data);

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
