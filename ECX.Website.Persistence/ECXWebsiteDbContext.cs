using ECX.Website.Domain;
using ECX.Website.Domain.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECX.Website.Persistence
{
    public class ECXWebsiteDbContext :DbContext    
    {
        public ECXWebsiteDbContext(DbContextOptions<ECXWebsiteDbContext> options) : base(options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ECXWebsiteDbContext).Assembly);
        }
        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            foreach (var entity in ChangeTracker.Entries<BaseDomainEntity>())
            {
                entity.Entity.UpdatedDate = DateTime.Now;
                if(entity.State == EntityState.Added)
                {
                    entity.Entity.CreatedDate = DateTime.Now;
                    entity.Entity.IsActive = true;
                }
                
            }
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }
        public DbSet<Commodity> Commodities { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<Page> Pages { get; set; }
        public DbSet<PageCatagory> PageCatagories { get; set; }
        public DbSet<BoardOfDirector> BoardOfDirectors { get; set; }
        public DbSet<Image> Images { get; set; }
    }
}
