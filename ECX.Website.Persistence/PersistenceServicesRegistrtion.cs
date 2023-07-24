
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

            return services;
        }

    }
}
