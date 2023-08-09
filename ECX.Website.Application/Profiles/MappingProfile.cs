using AutoMapper;
using ECX.Website.Application.DTOs.Commodity;
using ECX.Website.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECX.Website.Application.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CommodityDto,Commodity>().ReverseMap();

            CreateMap<CommodityFormDto, CommodityDto>()
                .ForMember(dest => dest.LangId, act => act.MapFrom(src => src.LangId))
                .ForMember(dest => dest.Name, act => act.MapFrom(src => src.Name))
                .ForMember(dest => dest.CreatedBy, act => act.MapFrom(src => src.CreatedBy))
                .ForMember(dest => dest.UpdatedBy, act => act.MapFrom(src => src.UpdatedBy))
                .ForMember(dest => dest.Description, act => act.MapFrom(src => src.Description)
                ).ReverseMap();

        }
    }
}
