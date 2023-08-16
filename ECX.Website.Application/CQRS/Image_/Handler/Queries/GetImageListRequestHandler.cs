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
    public class GetImageListRequestHandler : IRequestHandler<GetImageListRequest, BaseCommonResponse>
    {
        private IImageRepository _ImageRepository;
        private IMapper _mapper;
        public GetImageListRequestHandler(IImageRepository ImageRepository, IMapper mapper)
        {
            _ImageRepository = ImageRepository;
            _mapper = mapper;
        }
        public async Task<BaseCommonResponse> Handle(GetImageListRequest request, CancellationToken cancellationToken)
        {
            var response = new BaseCommonResponse();
            var data = await _ImageRepository.GetAll();

            response.Success = true;
            response.Data = _mapper.Map<List<ImageDto>>(data);
            response.Status = "200";

            return response;
        }
    }
}
