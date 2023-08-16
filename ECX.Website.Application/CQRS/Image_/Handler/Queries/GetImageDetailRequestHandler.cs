using AutoMapper;
using ECX.Website.Application.Contracts.Persistence;
using ECX.Website.Application.CQRS.Image_.Request.Command;
using ECX.Website.Application.DTOs.Image;
using ECX.Website.Application.DTOs.Image.Validators;
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
using ECX.Website.Application.CQRS.Image_.Request.Queries;

namespace ECX.Website.Application.CQRS.Image_.Handler.Queries
{
    public class GetImageDetailRequestHandler : IRequestHandler<GetImageDetailRequest, BaseCommonResponse>
    {
        private IImageRepository _imageRepository;
        private IMapper _mapper;
        
        public GetImageDetailRequestHandler(IImageRepository imageRepository, IMapper mapper)
        {
            _imageRepository = imageRepository;
            _mapper = mapper;
        }
        public async Task<BaseCommonResponse> Handle(GetImageDetailRequest request, CancellationToken cancellationToken)
        {
            var response = new BaseCommonResponse();
            var data = await _imageRepository.GetById(request.Id);
            if (data != null)
            {
                response.Success = true;
                response.Data = _mapper.Map<ImageDto>(data);
                response.Status = "200";
            }
            else
            {
                response.Success = false;
                response.Message = new NotFoundException(
                    nameof(Image), request.Id).Message.ToString();
                response.Status = "404";
            }
            return response;
        }
    }
}
