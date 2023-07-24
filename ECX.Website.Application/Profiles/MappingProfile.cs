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
        }
    }
}
