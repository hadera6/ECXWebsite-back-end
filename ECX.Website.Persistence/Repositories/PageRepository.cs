using ECX.Website.Application.Contracts.Persistence;
using ECX.Website.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace ECX.Website.Persistence.Repositories
{
    public class PageRepository : GenericRepository<Page>, IPageRepository
    {
        private readonly ECXWebsiteDbContext _context;

        public PageRepository(ECXWebsiteDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Page>> GetByPageCatagoryLangId(string catagoryId,string langId)
        { 
            return _context.Set<Page>().Where(
                p => p.CatagoryId == catagoryId && p.LangId == langId
                ).ToList();  
        }
    }
}
