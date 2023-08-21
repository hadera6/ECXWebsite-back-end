using ECX.Website.Application.Contracts.Persistence;
using ECX.Website.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECX.Website.Persistence.Repositories
{
    public class ResearchRepository : GenericRepository<Research>, IResearchRepository
    {
        private readonly ECXWebsiteDbContext _context;

        public ResearchRepository(ECXWebsiteDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
