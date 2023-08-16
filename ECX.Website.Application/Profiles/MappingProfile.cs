using AutoMapper;
using ECX.Website.Application.DTOs.Commodity;
using ECX.Website.Application.DTOs.News;
using ECX.Website.Application.DTOs.Page;
using ECX.Website.Application.DTOs.PageCatagory;
using ECX.Website.Application.DTOs.BoardOfDirector;
using ECX.Website.Application.DTOs.Image;
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

            CreateMap<NewsDto,News>().ReverseMap();

            CreateMap<NewsFormDto, NewsDto>()
                .ForMember(dest => dest.LangId, act => act.MapFrom(src => src.LangId))
                .ForMember(dest => dest.Title, act => act.MapFrom(src => src.Title))
                .ForMember(dest => dest.CreatedBy, act => act.MapFrom(src => src.CreatedBy))
                .ForMember(dest => dest.UpdatedBy, act => act.MapFrom(src => src.UpdatedBy))
                .ForMember(dest => dest.Description, act => act.MapFrom(src => src.Description))
                .ForMember(dest => dest.ExpDate, act => act.MapFrom(src => src.ExpDate))
                .ForMember(dest => dest.Source, act => act.MapFrom(src => src.Source)
                ).ReverseMap();

            CreateMap<PageDto,Page>().ReverseMap();

            CreateMap<PageFormDto, PageDto>()
                .ForMember(dest => dest.LangId, act => act.MapFrom(src => src.LangId))
                .ForMember(dest => dest.Title, act => act.MapFrom(src => src.Title))
                .ForMember(dest => dest.CreatedBy, act => act.MapFrom(src => src.CreatedBy))
                .ForMember(dest => dest.UpdatedBy, act => act.MapFrom(src => src.UpdatedBy))
                .ForMember(dest => dest.CatagoryId, act => act.MapFrom(src => src.CatagoryId))
                .ForMember(dest => dest.Description, act => act.MapFrom(src => src.Description)
                ).ReverseMap();
            
            CreateMap<PageCatagoryDto,PageCatagory>().ReverseMap();

            CreateMap<PageCatagoryFormDto, PageCatagoryDto>()
                .ForMember(dest => dest.LangId, act => act.MapFrom(src => src.LangId))
                .ForMember(dest => dest.Title, act => act.MapFrom(src => src.Title))
                .ForMember(dest => dest.CreatedBy, act => act.MapFrom(src => src.CreatedBy))
                .ForMember(dest => dest.UpdatedBy, act => act.MapFrom(src => src.UpdatedBy))
                .ForMember(dest => dest.Description, act => act.MapFrom(src => src.Description)
                ).ReverseMap();

            CreateMap<BoardOfDirectorDto,BoardOfDirector>().ReverseMap();

            CreateMap<BoardOfDirectorFormDto, BoardOfDirectorDto>()
                .ForMember(dest => dest.LangId, act => act.MapFrom(src => src.LangId))
                .ForMember(dest => dest.Name, act => act.MapFrom(src => src.Name))
                .ForMember(dest => dest.CreatedBy, act => act.MapFrom(src => src.CreatedBy))
                .ForMember(dest => dest.UpdatedBy, act => act.MapFrom(src => src.UpdatedBy))
                .ForMember(dest => dest.Description, act => act.MapFrom(src => src.Description)
                ).ReverseMap();

            CreateMap<ImageDto,Image>().ReverseMap();

            CreateMap<ImageFormDto, ImageDto>()
                .ForMember(dest => dest.LangId, act => act.MapFrom(src => src.LangId))
                .ForMember(dest => dest.Title, act => act.MapFrom(src => src.Title))
                .ForMember(dest => dest.CreatedBy, act => act.MapFrom(src => src.CreatedBy))
                .ForMember(dest => dest.UpdatedBy, act => act.MapFrom(src => src.UpdatedBy))
                .ForMember(dest => dest.Caption, act => act.MapFrom(src => src.Caption)
                ).ReverseMap();
                
        }
    }
}
