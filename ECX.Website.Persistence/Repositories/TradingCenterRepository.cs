using ECX.Website.Application.Contracts.Persistence;
using ECX.Website.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECX.Website.Persistence.Repositories
{
    public class TradingCenterRepository : GenericRepository<TradingCenter>, ITradingCenterRepository
    {
        private readonly ECXWebsiteDbContext _context;

        public TradingCenterRepository(ECXWebsiteDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
