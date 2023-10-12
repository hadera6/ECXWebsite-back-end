using ECX.Website.Application.Contracts.Persistence;
using ECX.Website.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECX.Website.Persistence.Repositories
{
    public class BlogRepository : GenericRepository<Blog>, IBlogRepository
    {
        private readonly ECXWebsiteDbContext _context;

        public BlogRepository(ECXWebsiteDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Blog>> GetAllBlogs()
        { 
            return await _context.Blogs.FromSqlRaw("usp_SelectTable").ToListAsync(); 
        }
    }
}
