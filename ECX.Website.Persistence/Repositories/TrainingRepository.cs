using ECX.Website.Application.Contracts.Persistence;
using ECX.Website.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECX.Website.Persistence.Repositories
{
    public class TrainingRepository : GenericRepository<Training>, ITrainingRepository
    {
        private readonly ECXWebsiteDbContext _context;

        public TrainingRepository(ECXWebsiteDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
