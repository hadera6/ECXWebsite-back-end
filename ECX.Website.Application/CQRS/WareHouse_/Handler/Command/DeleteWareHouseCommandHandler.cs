using AutoMapper;
using ECX.Website.Application.CQRS.WareHouse_.Request.Command;
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

namespace ECX.Website.Application.CQRS.WareHouse_.Handler.Command
{
    public class DeleteWareHouseCommandHandler : IRequestHandler<DeleteWareHouseCommand, BaseCommonResponse>
    {
        
        private IWareHouseRepository _wareHouseRepository;
        private IMapper _mapper;
        public DeleteWareHouseCommandHandler(IWareHouseRepository wareHouseRepository, IMapper mapper)
        {
            _wareHouseRepository = wareHouseRepository;
            _mapper = mapper;
        }
        public async Task<BaseCommonResponse> Handle(DeleteWareHouseCommand request, CancellationToken cancellationToken)
        {
            var data = await _wareHouseRepository.GetById(request.Id);
            var response = new BaseCommonResponse();

            if (data == null)
            {
                response.Success = false;
                response.Message = new NotFoundException(
                            nameof(WareHouse), request.Id).Message.ToString();
                response.Status = "404";
            }
            else
            {
                await _wareHouseRepository.Delete(data);

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
