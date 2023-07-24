using ECX.Website.Application.Contracts.Persistence;
using ECX.Website.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECX.Website.Persistence.Repositories
{
    public class CommodityRepository : GenericRepository<Commodity>, ICommodityRepository
    {
        private readonly ECXWebsiteDbContext _context;

        public CommodityRepository(ECXWebsiteDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
