using ECX.Website.Application.Contracts.Persistence;
using ECX.Website.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECX.Website.Persistence
{
    public static partial class PersistenceServicesRegistrtion
    {
        public static IServiceCollection ConfigurePersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ECXWebsiteDbContext>
                (options => options.UseSqlServer(configuration.GetConnectionString("ECXWebsiteConnectionString")));
            
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<ICommodityRepository, CommodityRepository>();
            services.AddScoped<INewsRepository, NewsRepository>();
            services.AddScoped<IPageRepository, PageRepository>();
            services.AddScoped<IPageCatagoryRepository, PageCatagoryRepository>();
            services.AddScoped<IBoardOfDirectorRepository, BoardOfDirectorRepository>();
            services.AddScoped<IImageRepository, ImageRepository>();
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IAnnouncementRepository, AnnouncementRepository>();
            services.AddScoped<IApplicantRepository, ApplicantRepository>();
            services.AddScoped<IBlogRepository, BlogRepository>();
            services.AddScoped<IBrochureRepository, BrochureRepository>();
            services.AddScoped<IContractFileRepository, ContractFileRepository>();
            services.AddScoped<IExternalLinkRepository, ExternalLinkRepository>();
            services.AddScoped<IFaqRepository, FaqRepository>();
            services.AddScoped<IFeedBackRepository, FeedBackRepository>();
            services.AddScoped<ILanguageRepository, LanguageRepository>();
            services.AddScoped<IMessageRepository, MessageRepository>();
            services.AddScoped<IPublicationRepository, PublicationRepository>();
            services.AddScoped<IResearchRepository, ResearchRepository>();
            services.AddScoped<ISocialMediaRepository, SocialMediaRepository>();
            services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();
            services.AddScoped<ITradingCenterRepository, TradingCenterRepository>();
            services.AddScoped<ITrainingRepository, TrainingRepository>();
            services.AddScoped<ITrainingDocRepository, TrainingDocRepository>();
            services.AddScoped<IVacancyRepository, VacancyRepository>();
            services.AddScoped<IVideoRepository, VideoRepository>();
            services.AddScoped<IWareHouseRepository, WareHouseRepository>();

            
            return services;
        }

    }
}
