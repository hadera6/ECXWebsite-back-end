using AutoMapper;
using ECX.Website.Application.DTOs.Commodity;
using ECX.Website.Application.DTOs.News;
using ECX.Website.Application.DTOs.Page;
using ECX.Website.Application.DTOs.PageCatagory;
using ECX.Website.Application.DTOs.BoardOfDirector;
using ECX.Website.Application.DTOs.Image;
using ECX.Website.Application.DTOs.Account;
using ECX.Website.Application.DTOs.Announcement;
using ECX.Website.Application.DTOs.Applicant;
using ECX.Website.Application.DTOs.Blog;
using ECX.Website.Application.DTOs.Brochure;
using ECX.Website.Application.DTOs.ContractFile;
using ECX.Website.Application.DTOs.ExternalLink;
using ECX.Website.Application.DTOs.Faq;
using ECX.Website.Application.DTOs.FeedBack;
using ECX.Website.Application.DTOs.Language;
using ECX.Website.Application.DTOs.Message;
using ECX.Website.Application.DTOs.Publication;
using ECX.Website.Application.DTOs.Research;
using ECX.Website.Application.DTOs.SocialMedia;
using ECX.Website.Application.DTOs.Subscription;
using ECX.Website.Application.DTOs.TradingCenter;
using ECX.Website.Application.DTOs.Training;
using ECX.Website.Application.DTOs.TrainingDoc;
using ECX.Website.Application.DTOs.Vacancy;
using ECX.Website.Application.DTOs.Video;
using ECX.Website.Application.DTOs.WareHouse;
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
            CreateMap<CommodityDto, Commodity>().ReverseMap();

            CreateMap<CommodityFormDto, CommodityDto>().ReverseMap();

            CreateMap<NewsDto,News>().ReverseMap();

            CreateMap<NewsFormDto, NewsDto>().ReverseMap();

            CreateMap<PageDto,Page>().ReverseMap();

            CreateMap<PageFormDto, PageDto>().ReverseMap();
            
            CreateMap<PageCatagoryDto,PageCatagory>().ReverseMap();

            CreateMap<PageCatagoryFormDto, PageCatagoryDto>().ReverseMap();

            CreateMap<BoardOfDirectorDto,BoardOfDirector>().ReverseMap();

            CreateMap<BoardOfDirectorFormDto, BoardOfDirectorDto>().ReverseMap();

            CreateMap<ImageDto,Image>().ReverseMap();

            CreateMap<ImageFormDto, ImageDto>().ReverseMap();
            
            CreateMap<WareHouseDto,WareHouse>().ReverseMap();
            
            CreateMap<WareHouseFormDto, WareHouseDto>().ReverseMap();
            
            CreateMap<VideoDto,Video>().ReverseMap();
            
            CreateMap<VideoFormDto, VideoDto>().ReverseMap();
            
            CreateMap<VacancyDto,Vacancy>().ReverseMap();
            
            CreateMap<VacancyFormDto, VacancyDto>().ReverseMap();
            
            CreateMap<TrainingDocDto,TrainingDoc>().ReverseMap();
            
            CreateMap<TrainingDocFormDto, TrainingDocDto>().ReverseMap();
            
            CreateMap<TrainingDto,Training>().ReverseMap();
            
            CreateMap<TrainingFormDto, TrainingDto>().ReverseMap();
            
            CreateMap<TradingCenterDto,TradingCenter>().ReverseMap();
            
            CreateMap<TradingCenterFormDto, TradingCenterDto>().ReverseMap();
            
            CreateMap<SubscriptionDto,Subscription>().ReverseMap();
            
            CreateMap<SubscriptionFormDto, SubscriptionDto>().ReverseMap();
            
            CreateMap<SocialMediaDto,SocialMedia>().ReverseMap();
            
            CreateMap<SocialMediaFormDto, SocialMediaDto>().ReverseMap();
            
            CreateMap<ResearchDto,Research>().ReverseMap();
            
            CreateMap<ResearchFormDto, ResearchDto>().ReverseMap();
            
            CreateMap<PublicationDto,Publication>().ReverseMap();
            
            CreateMap<PublicationFormDto, PublicationDto>().ReverseMap();
            
            CreateMap<MessageDto,Message>().ReverseMap();
            
            CreateMap<MessageFormDto, MessageDto>().ReverseMap();
            
            CreateMap<LanguageDto,Language>().ReverseMap();
            
            CreateMap<LanguageFormDto, LanguageDto>().ReverseMap();
            
            CreateMap<FeedBackDto,FeedBack>().ReverseMap();
            
            CreateMap<FeedBackFormDto, FeedBackDto>().ReverseMap();
            
            CreateMap<FaqDto,Faq>().ReverseMap();
            
            CreateMap<FaqFormDto, FaqDto>().ReverseMap();
            
            CreateMap<ExternalLinkDto,ExternalLink>().ReverseMap();
            
            CreateMap<ExternalLinkFormDto, ExternalLinkDto>().ReverseMap();
            
            CreateMap<ContractFileDto,ContractFile>().ReverseMap();
            
            CreateMap<ContractFileFormDto, ContractFileDto>().ReverseMap();
            
            CreateMap<BrochureDto,Brochure>().ReverseMap();
            
            CreateMap<BrochureFormDto, BrochureDto>().ReverseMap();
            
            CreateMap<BlogDto,Blog>().ReverseMap();
            
            CreateMap<BlogFormDto, BlogDto>().ReverseMap();
            
            CreateMap<ApplicantDto,Applicant>().ReverseMap();
            
            CreateMap<ApplicantFormDto, ApplicantDto>().ReverseMap();
            
            CreateMap<AnnouncementDto,Announcement>().ReverseMap();
            
            CreateMap<AnnouncementFormDto, AnnouncementDto>().ReverseMap();
            
            CreateMap<AccountDto,Account>().ReverseMap();
            
            CreateMap<AccountFormDto, AccountDto>().ReverseMap();
   
        }
    }
}
