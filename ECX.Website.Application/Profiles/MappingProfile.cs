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
            CreateMap<Commodity, CommodityDto>().ReverseMap();

            CreateMap<Commodity, CommodityFormDto>()
                .ForMember(dest => dest.Name, act => act.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, act => act.MapFrom(src => src.Description))
                .ForMember(dest => dest.Img, act => act.MapFrom(src => src.Img)).ReverseMap();
        }
    }
}
