using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace ECX.Website.Persistence
{
    public static partial class PersistenceServiceRegistrtion
    {
        public class ECXWebsiteDbContextFactory
            : IDesignTimeDbContextFactory<ECXWebsiteDbContext>
        {
            public ECXWebsiteDbContext CreateDbContext(string[] args)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
                var builder = new DbContextOptionsBuilder<ECXWebsiteDbContext>();
                var connectionString = configuration.GetConnectionString("ECXWebsiteConnectionString");
                builder.UseSqlServer(connectionString);
                return new ECXWebsiteDbContext(builder.Options);
            }
        }
    }
}
