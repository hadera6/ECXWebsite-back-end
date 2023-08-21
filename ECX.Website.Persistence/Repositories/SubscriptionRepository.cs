using ECX.Website.Application.Contracts.Persistence;
using ECX.Website.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECX.Website.Persistence.Repositories
{
    public class SubscriptionRepository : GenericRepository<Subscription>, ISubscriptionRepository
    {
        private readonly ECXWebsiteDbContext _context;

        public SubscriptionRepository(ECXWebsiteDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
